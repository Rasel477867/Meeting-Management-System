using MMSCore;
using MMSCore.NotMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSService.Contact
{
    public interface IBookingService
    {
        public Task<bool> IsBookingAvailableAsync(Booking booking);
        public Task<bool> AddAsync(Booking Entity);
        Task<List<Booking>> GetBookingList();
       public Task<(IEnumerable<Booking> bookings, int TotalCount)> GetBookingAsync(BookingQuery bookingQuery, int page, int pageSize);
    }
}
