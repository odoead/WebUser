using WebUser.Domain.exceptions;

namespace WebUser.features.Coupon.Exceptions
{
    public class CouponNotFoundException : NotFoundException
    {
        public CouponNotFoundException(int id)
            : base($"Coupon with ID {id} doesnt exists") { }
    }
}
