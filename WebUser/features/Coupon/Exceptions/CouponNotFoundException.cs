namespace WebUser.features.Coupon.Exceptions
{
    public class CouponNotFoundException : Exception
    {
        public CouponNotFoundException(int id) : base($"Coupon with ID {id} doesnt exists")
        {

        }
    }
}
