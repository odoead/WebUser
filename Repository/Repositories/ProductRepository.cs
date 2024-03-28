using Entities;
using Entities.data;
using Interfaces.IRepository;

namespace WebUser.Repository
{
    public class ProductRepository : IProductRepo
    {
        private AppDbContext DBContext;
        public ProductRepository(AppDbContext db)
        {
            DBContext = db;
        }
        public Task<bool> CreateAsync(ProductDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(ProductDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDTO> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ProductDTO>> GetAllAsync()
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

        public Task<bool> UpdateAsync(ProductDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
