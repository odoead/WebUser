using WebUser.shared;
using E = WebUser.Domain.entities;
namespace WebUser.features.Order.Interfaces
{
    public interface IOrderService
    {
        public void Create(E.Order order);
        public void AddCoupon(E.Order order, E.Coupon coupon);

        public void AddPoints(E.Order order, int pointsValue);

        public bool IsProcessed(ObjectID<E.Order> Id);

        public void Delete(E.Order Order);
        public Task<ICollection<E.Order>> GetAllAsync();
        public Task<E.Order> GetByIdAsync(ObjectID<E.Order> Id);
        public Task<bool> IsExistsAsync(ObjectID<E.Order> Id);
        public  Task<ICollection<E.Order>>? GetByUserIdAsync(ObjectID<E.User> UserId);


    }
}
