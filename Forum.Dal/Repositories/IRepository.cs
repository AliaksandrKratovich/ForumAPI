using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Dal.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task Add(T obj);
        Task<bool> Update(T obj);
        Task<bool> Remove(Guid id);
        Task<T> Find(Guid id);
        Task<IEnumerable<T>> GetAll();

    }
}
