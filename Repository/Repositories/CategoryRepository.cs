using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DBData;
using Entities;
using Interfaces.IRepository;

namespace Repository.Repositories
{
    internal class CategoryRepository : ICategoryRepo
    {
        private AppDbContext DBContext;

        public CategoryRepository(AppDbContext db)
        {
            DBContext = db;
        }

        public Task<bool> CreateAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task<Category> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Category>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryWithDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
