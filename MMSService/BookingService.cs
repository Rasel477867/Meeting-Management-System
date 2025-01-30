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

        public async Task<bool> IsBookingAvailableAsync(Booking booking)
        {
            var existingBookings = _unitOfWork.BookingRepository.GetBookingListAsync();
       
            //No Repeat false
            if (booking.RepetitionOption == RepeatOption.NoRepeat)
            {
                return ! await existingBookings.AnyAsync(x =>
                    x.BookingDate == booking.BookingDate &&
                    ((x.StartTime < booking.StartTime && booking.StartTime < x.EndTime) ||
                     (x.StartTime < booking.EndTime && booking.EndTime < x.EndTime) ||
                     (x.StartTime == booking.StartTime && x.EndTime == booking.EndTime))); // Check exact same time
            }
            // Daily Book false
            if (booking.RepetitionOption == RepeatOption.Daily && booking.EndRepeatedDate.HasValue)
            {
                return ! await existingBookings.AnyAsync(x =>
                    x.RepetitionOption == RepeatOption.Daily &&
                    x.BookingDate <= booking.BookingDate &&
                    x.EndRepeatedDate >= booking.BookingDate &&
                    x.StartTime < booking.EndTime &&
                    x.EndTime > booking.StartTime);
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
