using Data.DBData;
using Entities;
using Interfaces.IRepository;

namespace WebUser.Repository
{
    public class OrderRepository : IOrderRepo
    {
        private AppDbContext DBContext;
        public OrderRepository(AppDbContext db)
        {
            DBContext = db;
        }
        public Task<bool> CreateAsync(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task<Order> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Order>> GetAllAsync()
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

        public Task<bool> UpdateAsync(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
