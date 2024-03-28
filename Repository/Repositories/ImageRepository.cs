using Data.DBData;
using Entities;
using Interfaces.IRepository;

namespace WebUser.Repository
{
    public class ImageRepository : IImageRepo
    {
        private AppDbContext DBContext;
        public ImageRepository(AppDbContext db)
        {
            DBContext = db;
        }
        public Task<bool> CreateAsync(Image entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Image entity)
        {
            throw new NotImplementedException();
        }

        public Task<Image> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Image>> GetAllAsync()
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

        public Task<bool> UpdateAsync(Image entity)
        {
            throw new NotImplementedException();
        }
    }
}
