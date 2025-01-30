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
    public class BookingUnitOfWork : UnitOfWork, IBookingUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public BookingUnitOfWork(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public IBookingRepository BookingRepository
        {
            get
            {
                return new BookingRepository(_context);
            }
        }

        
    }
}
