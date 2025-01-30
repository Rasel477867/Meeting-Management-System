using MMSRepository.Contacts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSRepository.Contacts
{
    public interface IBookingUnitOfWork:IUnitOfWork
    {
        IBookingRepository BookingRepository { get; }
    }
}
