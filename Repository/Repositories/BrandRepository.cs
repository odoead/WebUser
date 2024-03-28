using Data.DBData;
using Entities;
using Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;

namespace WebUser.Repository
{
    public class BrandRepository : IBrandRepo 
    {
        private AppDbContext DBContext;
        public BrandRepository(AppDbContext db)
        {
            DBContext = db;
        }
        public Task<bool> CreateAsync(BrandDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(BrandDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<BrandDTO> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<BrandDTO>> GetAllAsync()
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

        public Task<bool> UpdateAsync(BrandDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
