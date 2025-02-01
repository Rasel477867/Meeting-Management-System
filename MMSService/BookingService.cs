﻿using Microsoft.EntityFrameworkCore;
using MMSCore;
using MMSCore.Enum;
using MMSRepository.Contacts;
using MMSService.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSService
{
    public class BookingService : IBookingService
    {
        private readonly IBookingUnitOfWork _unitOfWork;
        public BookingService(IBookingUnitOfWork bookingUnitOfWork)
        {
            _unitOfWork = bookingUnitOfWork;
        }
        public async Task<bool> AddAsync(Booking Entity)
        {
           await _unitOfWork.BookingRepository.Add(Entity);
           return await _unitOfWork.CompleteAsync();
        }

        public Task<List<Booking>> GetBookingList()
        {
            return _unitOfWork.BookingRepository.GetBookingList();
        }

        public async Task<bool> IsBookingAvailableAsync(Booking booking)
        {
            var existingBookings = _unitOfWork.BookingRepository.GetBookingListAsync();
       
            //No Repeat false
            if (booking.RepetitionOption == RepeatOption.NoRepeat)
            {
                var state=! await existingBookings.AnyAsync(x =>
                    x.BookingDate == booking.BookingDate &&
                    ((x.EndTime > booking.StartTime && x.EndTime < booking.EndTime) ||
                     (x.StartTime > booking.StartTime && x.StartTime < booking.EndTime) ||
                     (x.StartTime==booking.StartTime && x.EndTime==booking.EndTime)||
                    (x.StartTime<booking.StartTime && x.EndTime>booking.EndTime))); // Check exact same time
                if (state == false)
                {
                    return false;
                }
                else
                {
                    var list= existingBookings.ToList();
                    for(var i=0; i<list.Count; i++){
                        if (list[i].RepetitionOption == RepeatOption.Daily) {
                            if (list[i].BookingDate<=booking.BookingDate && booking.BookingDate <= list[i].EndRepeatedDate)
                            {

                                if (list[i].StartTime == booking.StartTime && list[i].EndTime == booking.EndTime)
                                {
                                    return false;
                                }
                                else if (list[i].EndTime > booking.StartTime && list[i].EndTime < booking.EndTime)
                                {
                                    return false;
                                }
                                else if (list[i].StartTime > booking.StartTime && list[i].StartTime < booking.EndTime)
                                {
                                    return false;
                                }
                                else if (list[i].StartTime < booking.StartTime && list[i].EndTime > booking.EndTime)
                                {
                                    return false;
                                }
                            }
                        
                        }
                        else if (list[i].RepetitionOption == RepeatOption.Weekly)
                        {
                            if (list[i].BookingDate <= booking.BookingDate && booking.BookingDate <= list[i].EndRepeatedDate)
                            {
                                var inputday = (DaysofworkEnum)(1 << (int)booking.BookingDate.DayOfWeek); // Get bitwise value of the weekday
                                var existday = list[i].DaysToRepeatedOn;
                                var ans = ((int?)existday ?? 0) & (int)inputday;
                                if (ans != 0)
                                {
                                    if (list[i].StartTime == booking.StartTime && list[i].EndTime == booking.EndTime)
                                    {
                                        return false;
                                    }
                                    else if (list[i].EndTime > booking.StartTime && list[i].EndTime < booking.EndTime)
                                    {
                                        return false;
                                    }
                                    else if (list[i].StartTime > booking.StartTime && list[i].StartTime < booking.EndTime)
                                    {
                                        return false;
                                    }
                                    else if (list[i].StartTime < booking.StartTime && list[i].EndTime > booking.EndTime)
                                    {
                                        return false;
                                    }
                                }


                            }
                        }


                    }
                   
                }
            }
            // Daily Book false
            else if (booking.RepetitionOption == RepeatOption.Daily && booking.EndRepeatedDate.HasValue)
            {
                var state = !await existingBookings.AnyAsync(x =>
                    x.RepetitionOption == RepeatOption.Daily &&
                    x.BookingDate <= booking.BookingDate &&
                    x.EndRepeatedDate >= booking.BookingDate &&
                    x.StartTime < booking.EndTime &&
                    x.EndTime > booking.StartTime);
                if (state == false)
                {
                    return false;
                }
                else
                {
                    var list=existingBookings.ToList();
                    for(int i=0; i<list.Count; i++)
                    {
                        // Daily case exist NoRepeat option duplicate check
                        if (list[i].RepetitionOption == RepeatOption.NoRepeat)
                        {

                            if (booking.BookingDate <= list[i].BookingDate && list[i].BookingDate <= booking.EndRepeatedDate)
                            {
                                if (list[i].StartTime == booking.StartTime && list[i].EndTime == booking.EndTime)
                                {
                                    return false;
                                }
                                else if (list[i].EndTime > booking.StartTime && list[i].EndTime < booking.EndTime)
                                {
                                    return false;
                                }
                                else if (list[i].StartTime > booking.StartTime && list[i].StartTime < booking.EndTime)
                                {
                                    return false;
                                }
                                else if (list[i].StartTime < booking.StartTime && list[i].EndTime > booking.EndTime)
                                {
                                    return false;
                                }
                            }

                        }// Daily case exist weekly option duplicate check
                        else if (list[i].RepetitionOption==RepeatOption.Weekly)
                        {
                            if ((list[i].BookingDate<=booking.BookingDate && booking.BookingDate <= list[i].EndRepeatedDate) || (list[i].BookingDate<=booking.EndRepeatedDate && booking.EndRepeatedDate <= list[i].EndRepeatedDate))
                            {


                                var existday = list[i].DaysToRepeatedOn;
                                DaysofworkEnum InputdaysFlag = DaysofworkEnum.None; // Initial empty flag

                                for (DateTime date = booking.BookingDate; date <= booking.EndRepeatedDate; date = date.AddDays(1))
                                {
                                    var dayEnum = (DaysofworkEnum)(1 << (int)date.DayOfWeek);
                                    InputdaysFlag |= dayEnum; // Add to the flag using bitwise OR
                                }
                                var ans = (int)InputdaysFlag & (int)(existday ?? 0);
                                if (ans != 0) {
                                
                                    if (list[i].StartTime==booking.StartTime && list[i].EndTime == booking.EndTime)
                                    {
                                        return false;
                                    }
                                    else if (list[i].EndTime>booking.StartTime && list[i].EndTime < booking.EndTime)
                                    {
                                        return false;
                                    }
                                    else if (list[i].StartTime>booking.StartTime && list[i].StartTime < booking.EndTime)
                                    {
                                        return false;
                                    }
                                    else if (list[i].StartTime<booking.StartTime && list[i].EndTime> booking.EndTime) {
                                       return false;
                                    }

                                }
                            }
                        }
                    }
                }
            }

          // Weekly Option false
            else if (booking.RepetitionOption == RepeatOption.Weekly && booking.EndRepeatedDate.HasValue)
            {
                return !await existingBookings.AnyAsync(x =>
                    x.RepetitionOption == RepeatOption.Weekly &&
                    x.BookingDate <= booking.BookingDate &&
                    x.EndRepeatedDate >= booking.BookingDate &&
                    (x.DaysToRepeatedOn & booking.DaysToRepeatedOn) != 0 && 
                    x.StartTime < booking.EndTime &&
                    x.EndTime > booking.StartTime);
            }
            // Other Return  ture for added data
            return true; 
        }
    }
}
