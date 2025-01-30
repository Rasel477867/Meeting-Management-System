using MMSRepository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSRepository.Contacts.Core
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _dbcontext;
        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _dbcontext = applicationDbContext;
        }
        public async Task<bool> CompleteAsync()
        {
            var sta= await _dbcontext.SaveChangesAsync() > 0;
            return sta;
        }
    }
}
