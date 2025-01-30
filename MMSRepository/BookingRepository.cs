using MMSCore;
using MMSRepository.Contacts;
using MMSRepository.Contacts.Core;
using MMSRepository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSRepository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public BookingRepository(ApplicationDbContext db) : base(db)
        {
            _dbcontext = db;
        }

        public IQueryable<Booking> GetBookingListAsync()
        {
            return _dbcontext.Bookings.AsQueryable();
        }
    }
}
