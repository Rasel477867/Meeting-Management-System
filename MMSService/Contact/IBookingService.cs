using MMSCore;
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
    }
}
