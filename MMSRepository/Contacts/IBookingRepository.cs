using MMSCore;
using MMSRepository.Contacts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSRepository.Contacts
{
    public interface IBookingRepository:IRepository<Booking>
    {
        IQueryable<Booking> GetBookingListAsync();
    }
}
