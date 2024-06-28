using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.OrderProduct.Interfaces;
using E = WebUser.Domain.entities;

namespace WebUser.features.OrderProduct
{
    public class OrderProductService : IOrderProductService
    {
        private readonly DB_Context context;

        public OrderProductService(DB_Context context)
        {
            this.context = context;
        }

        public async Task<ICollection<E.OrderProduct>> CreateOrderProdsFromCartItems(int cartId)
        {
            List<E.OrderProduct> orderProducts = new List<E.OrderProduct>();
            var cartItems = await context.CartItems.Where(q => q.CartID == cartId).ToListAsync();
            foreach (var item in cartItems)
            {
                orderProducts.Add(
                    new E.OrderProduct
                    {
                        Amount = item.Amount,
                        Product = item.Product,
                        ProductID = item.ProductID,
                        FinalPrice = item.Product.Price,
                    }
                );
            }
            context.CartItems.RemoveRange(cartItems);
            await context.OrderProducts.AddRangeAsync(orderProducts);
            return orderProducts;
        }

        public ICollection<E.OrderProduct> CreateOrderProdsFromCartItemsDiscounts(List<(E.CartItem items, double discount)> cartItemDiscounts)
        {
            var totalDiscountProduct = cartItemDiscounts
                .GroupBy(item => item.items)
                .Select(group => new
                {
                    Amount = group.Sum(item => item.items.Amount),
                    Product = group.Key,
                    TotalDiscount = group.Sum(item => item.discount) // Calculate sum of costs for each product
                });
            List<E.OrderProduct> orderProducts = new List<E.OrderProduct>();
            foreach (var item in totalDiscountProduct)
            {
                orderProducts.Add(
                    new E.OrderProduct
                    {
                        Amount = item.Amount,
                        Product = item.Product.Product,
                        FinalPrice = item.Product.Product.Price - item.TotalDiscount,
                    }
                );
            }
            return orderProducts;
        }
    }
}
