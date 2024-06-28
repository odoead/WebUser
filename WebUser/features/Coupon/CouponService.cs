using WebUser.Data;
using WebUser.features.Coupon.Interfaces;

namespace WebUser.features.Coupon
{
    public class CouponService : ICouponService
    {
        private readonly DB_Context _Context;

        public CouponService(DB_Context context)
        {
            _Context = context;
        }
    }
}
