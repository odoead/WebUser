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

        /// <summary>
        ///
        /// </summary>
        /// <param name="cartItemDiscounts"></param>
        /// <returns></returns>
        public ICollection<E.OrderProduct> CreateOrderProdsFromCartItemsDiscounts(
            List<(E.CartItem items, double discount)> cartItemDiscounts,
            List<(E.CartItem, E.Coupon)> activatedCoupons
        )
        {
            var totalDiscountProduct = cartItemDiscounts
                .GroupBy(discount => discount.items)
                .Select(group => new { Product = group.Key, TotalDiscount = group.Sum(item => item.discount) });
            List<E.OrderProduct> orderProducts = new List<E.OrderProduct>();
            foreach (var item in totalDiscountProduct)
            {
                var finalPrice = item.Product.Product.Price - item.TotalDiscount;
                var orderProduct = new E.OrderProduct
                {
                    Amount = item.Product.Amount,
                    Product = item.Product.Product,
                    ProductID = item.Product.ProductID,
                    FinalPrice = finalPrice > 0 ? finalPrice : 1,
                };

                var matchingCoupons = activatedCoupons.Where(c => c.Item1 == item.Product).Select(c => c.Item2);

                foreach (var coupon in matchingCoupons)
                {
                    orderProduct.ActivatedCoupons.Add(coupon);
                }
                orderProducts.Add(orderProduct);
            }


            return orderProducts;
        }
    }
}
