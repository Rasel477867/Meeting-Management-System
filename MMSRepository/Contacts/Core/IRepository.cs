using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSRepository.Contacts.Core
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(Guid id);
        Task<List<T>> GetAll();
        Task Update(T Entity);
        Task Delete(T Entity);
        Task Add(T Entity);
        IQueryable<T> GetAllAsync();


    }
}
