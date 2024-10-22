using E = WebUser.Domain.entities;

namespace WebUser.features.Order.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Applies Discount for cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns> list of items with their calculated discount values </returns>
        public List<(E.CartItem, double)> ApplyDiscount(IQueryable<E.Cart> cart);

        /// <summary>
        /// Applies coupons for cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="couponsCodes"></param>
        /// <returns>items with their calculated discount values</returns>
        public (List<(E.CartItem, double)>, List<(E.CartItem, E.Coupon)>) ApplyCoupons(IQueryable<E.Cart> cart, string couponsCodes);

        /// <summary>
        /// Determines necessary points amount for order
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pointsValue"></param>
        /// <returns>list of used points</returns>
        /// <exception cref="Exception"></exception>
        public ICollection<E.Point> ApplyPoints(E.User user, int pointsValue);

        /// <summary>
        ///Applies all active promotions to cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns>items with their calculated discount values and new point for user </returns>
        public (List<(E.CartItem, double)>, E.Point) ApplyPromos(IQueryable<E.Cart> cart);
        public bool IsProcessed(int Id);
    }
}
