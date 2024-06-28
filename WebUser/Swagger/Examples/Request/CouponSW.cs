using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class CouponSW : IExamplesProvider<Coupon>
    {
        public Coupon GetExamples() => throw new NotImplementedException();
    }
}
