using E = WebUser.Domain.entities;

namespace WebUser.features.OrderProduct.Interfaces
{
    public interface IOrderProductService
    {
        /// <summary>
        /// Moves products from cart to order
        /// </summary>
        /// <param name="CartId"></param>
        /// <returns></returns>
        public Task<ICollection<E.OrderProduct>> CreateOrderProdsFromCartItems(int CartId);

        /// <summary>
        /// Moves products from cart to order and calculates discount
        /// </summary>
        /// <param name="cartItemDiscounts"></param>
        /// <returns></returns>
        public ICollection<E.OrderProduct> CreateOrderProdsFromCartItemsDiscounts(
            List<(E.CartItem items, double discount)> cartItemDiscounts,
            List<(E.CartItem, E.Coupon)> activatedCoupons
        );
    }
}
