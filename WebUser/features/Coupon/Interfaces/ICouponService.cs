using WebUser.shared;
using E = WebUser.Domain.entities;
namespace WebUser.features.Coupon.Interfaces
{
    public interface ICouponService
    {
        public void Delete(E.Coupon coupon);
        public Task<ICollection<E.Coupon>> GetAllAsync();
        public Task<E.Coupon> GetByIdAsync(ObjectID<E.Coupon> Id);
        public Task<ICollection<E.Coupon>> GetByUserIdAsync(ObjectID<E.User> Id);
        public Task<ICollection<E.Coupon>> GetByOrderIdAsync(ObjectID<E.Order> Id);
        public Task<bool> IsExistsAsync(ObjectID<E.Coupon> Id);
        public void Update(E.Coupon coupon);

    }
}
