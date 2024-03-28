using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Coupon.Interfaces;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.Coupon
{
    public class CouponRepo : ICouponService
    {
        private DB_Context _Context;
        public CouponRepo(DB_Context context)
        {
            _Context = context;
        }
        
        public void Create(E.Coupon coupon)
        {
            _Context.coupons.Add(coupon);
        }

        public void Delete(E.Coupon coupon)
        {
            _Context.coupons.Remove(coupon);
        }

        public async Task<ICollection<E.Coupon>>? GetAllAsync()
        {
            return await _Context.coupons.ToListAsync();
        }

        public async Task<E.Coupon>? GetByIdAsync(ObjectID<E.Coupon> Id)
        {
            return await _Context.coupons.Where(q => q.ID == Id.Value).FirstOrDefaultAsync();
        }
        public async Task<ICollection<E.Coupon>>? GetByUserIdAsync(ObjectID<E.User> Id)
        {
            return await _Context.coupons.Where(q => q.User.Id == Id.Value).ToListAsync();
        }
        public async Task<ICollection<E.Coupon>>? GetByOrderIdAsync(ObjectID<E.Order> Id)
        {
            return await _Context.coupons.Where(q => q.Order.ID == Id.Value).ToListAsync();
        }
        public async Task<bool> IsExistsAsync(ObjectID<E.Coupon> Id)
        {
            return await _Context.coupons.AnyAsync(q => q.ID == Id.Value);
        }
        public void Update(E.Coupon coupon)
        {
            _Context.coupons.Update(coupon);
        }
    }
}
