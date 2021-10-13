using Books_api.Contracts;
using Books_api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books_api.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _Db;

        public AuthorRepository(ApplicationDbContext db)
        {
            _Db = db;
        }

        public async Task<bool> Create(Author entity)
        {
            await _Db.Authors.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {
            _Db.Authors.Remove(entity);
            return await Save();
        }

        public async Task<IList<Author>> FindAll()
        {
            var authors = await _Db.Authors.ToListAsync();
            return authors;
        }

        public async Task<Author> FindByID(int ID)
        {
            var author = await _Db.Authors.FindAsync(ID);
            return author;
        }

        public async Task<bool> isExits(int ID)
        {
            return await _Db.Authors.AnyAsync(a=>a.ID==ID);

        }

        public async Task<bool> Save()
        {
            var Changes =await _Db.SaveChangesAsync();
            return Changes > 0;
               
        }

        public async Task<bool> Update(Author entity)
        {
            _Db.Authors.Update(entity);
            return await Save();
        }
    }
}
