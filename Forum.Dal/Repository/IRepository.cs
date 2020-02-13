using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Dal.Repository
{
    public interface IRepository<T> where T : class
    {
        Task Add(T obj);
        Task Update(T obj);
        Task Remove(string id);
        Task<T> Find(string id);
        Task<IEnumerable<T>> GetAll();

    }
}
