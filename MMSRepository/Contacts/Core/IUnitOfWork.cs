using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSRepository.Contacts.Core
{
    public interface IUnitOfWork
    {
        Task<bool> CompleteAsync();
    }
}
