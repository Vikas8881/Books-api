using Books_api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books_api.Contracts
{
    public interface IBookRepository:IRepositoryBase<Book>
    {

    }
}
