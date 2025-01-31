using Microsoft.EntityFrameworkCore;
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
                    ((x.StartTime <= booking.StartTime && booking.StartTime <= x.EndTime) ||
                     (x.StartTime <= booking.EndTime && booking.EndTime <= x.EndTime))); // Check exact same time
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
                             
                                if (list[i].StartTime<=booking.StartTime && booking.StartTime <= list[i].EndTime)
                                {
                                    return false;
                                }
                                else if (list[i].StartTime <= booking.EndTime && booking.EndTime <= list[i].EndTime)
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
                                    if (list[i].StartTime <= booking.StartTime && booking.StartTime <= list[i].EndTime)
                                    {
                                        return false;
                                    }
                                    else if (list[i].StartTime <= booking.EndTime && booking.EndTime <= list[i].EndTime)
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
            if (booking.RepetitionOption == RepeatOption.Daily && booking.EndRepeatedDate.HasValue)
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
                        if (list[i].RepetitionOption == RepeatOption.NoRepeat)
                        {

                        }
                    }
                }
            }

          // Weekly Option false
            if (booking.RepetitionOption == RepeatOption.Weekly && booking.EndRepeatedDate.HasValue)
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
