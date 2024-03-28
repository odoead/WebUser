using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Order.DTO;
using WebUser.features.Order.Interfaces;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.Order
{
    public class OrderRepo : IOrderService
    {
        private DB_Context _Context;
        public OrderRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.Order order)
        {
            _Context.orders.Add(order);
        }
        public void AddCoupon(E.Order order, E.Coupon coupon)
        {
            if (coupon.ActiveFrom < DateTime.Now && coupon.ActiveTo > DateTime.Now && coupon.IsActivated == false && coupon.User == null)
            {
                order.ActivatedCoupons.Add(coupon);
                coupon.IsActivated = true;
            }
        }
        public void AddPoints(E.Order order, int pointsValue)
        {
            var points = _Context.points.Where(q => q.User.Id == order.User.Id).ToList();
            int pointsUsed = 0;
            foreach (var item in points)
            {
                //if(pointsValue
                if (item.isExpirable == false || (item.ExpireDate > DateTime.Now && pointsValue > pointsUsed))
                {
                    if (item.BalanceLeft > 0)
                    {
                        item.BalanceLeft -= Math.Min(item.BalanceLeft, pointsValue - pointsUsed);
                        pointsUsed += Math.Min(item.BalanceLeft, pointsValue - pointsUsed);
                    }

                }
                if (pointsValue <= pointsUsed)
                    break;
                item.IsUsed = true;
            }
            if (pointsValue < pointsUsed)
                throw new Exception("Not enought points");
            order.PointsUsed = pointsUsed;
        }
        public bool IsProcessed(ObjectID<E.Order> Id)
        {
            return _Context.orders.Any(q => q.ID == Id.Value && q.Status == true);
        }
        public void Delete(E.Order Order)
        {
            _Context.orders.Remove(Order);
        }

        public async Task<ICollection<E.Order>>? GetAllAsync()
        {
            return await _Context.orders.ToListAsync();
        }

        public async Task<E.Order>? GetByIdAsync(ObjectID<E.Order> Id)
        {
            return await _Context.orders.Where(q => q.ID == Id.Value).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(ObjectID<E.Order> Id)
        {
            return await _Context.orders.AnyAsync(q => q.ID == Id.Value);
        }
        public async Task<ICollection<E.Order>>? GetByUserIdAsync(ObjectID<E.User> UserId)
        {
            return await _Context.orders.Where(q => q.User.Id == UserId.Value).ToListAsync();
        }
        /*public void Update(E.Order Order)
        {
            _Context.orders.Update(Order);
        }*/

    }
}
