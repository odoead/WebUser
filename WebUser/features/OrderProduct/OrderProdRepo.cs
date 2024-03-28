using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.OrderProduct.Interfaces;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.OrderProduct
{
    public class OrderProductRepo : IOrderProductService
    {
        private DB_Context _Context;
        public OrderProductRepo(DB_Context context)
        {
            _Context = context;
        }
        public void CreateOrderProdFromCartItems(ObjectID<E.Cart> Id, E.Order order)
        {
            List<E.OrderProduct> orderProducts = new List<E.OrderProduct>();
            foreach (var item in _Context.cartItems.Where(q => q.CartId == Id.Value).ToList())
            {
                orderProducts.Add(new E.OrderProduct
                {
                    Amount = item.Amount,
                    Order = order,
                    Product = item.Product,
                    OrderId = order.ID,
                    ProductId = item.Product.ID,
                });
            }
            var itemsToRemove = _Context.cartItems.Where(q => q.CartId != Id.Value).ToList();
            _Context.cartItems.RemoveRange(itemsToRemove);
            _Context.orderProducts.AddRange(orderProducts);
        }

        public void Delete(E.OrderProduct OrderProduct)
        {
            _Context.orderProducts.Remove(OrderProduct);
        }

        public async Task<ICollection<E.OrderProduct>>? GetAllAsync()
        {
            return await _Context.orderProducts.ToListAsync();
        }

        public async Task<E.OrderProduct>? GetByIdAsync(ObjectID<E.OrderProduct> Id)
        {
            return await _Context.orderProducts.Where(q => q.ID == Id.Value).FirstOrDefaultAsync();
        }
        public async Task<ICollection<E.OrderProduct>>? GetByUserIdAsync(ObjectID<E.User> Id)
        {
            return await _Context.orderProducts.Where(q => q.Order.User.Id == Id.Value).ToListAsync();
        }

        public async Task<bool> IsExistsAsync(ObjectID<E.OrderProduct> Id)
        {
            return await _Context.orderProducts.AnyAsync(q => q.ID == Id.Value);
        }
        public void Update(E.OrderProduct OrderProduct)
        {
            _Context.orderProducts.Update(OrderProduct);
        }
    }
}
