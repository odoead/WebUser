using Entities;
using Entities.data;
using Interfaces.IRepository;

namespace WebUser.Repository
{
    public class UserRepository : IUserRepo
    {
        private AppDbContext DBContext;

        public UserRepository(AppDbContext db)
        {
            DBContext = db;
        }

        public Task<bool> CreateAsync(UserDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(UserDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserDTO>> GetAllAsync()
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

        public Task<bool> UpdateAsync(UserDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
