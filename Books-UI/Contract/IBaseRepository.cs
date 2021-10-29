using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books_UI.Contract
{
    public interface IBaseRepository<T> where T:class
    {
        Task<T> Get(string url, int ID);
        Task<IList<T>> Get(string url);
        Task<bool> Create(string url, T obj);
        Task<bool> Update(string url, T obj);
        Task<bool> Delete(string url, int ID);
    }
}
