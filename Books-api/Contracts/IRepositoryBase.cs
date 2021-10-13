using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books_api.Contracts
{
   public interface IRepositoryBase<T> where T:class
    {
        Task<IList<T>> FindAll();
        Task<T> FindByID(int ID);
        Task<bool> isExits(int ID);
        Task<bool> Create(T entity);

        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);

        Task<bool> Save();

    }
}
