using WebUser.features.AttributeName.Interfaces;
using WebUser.features.AttributeValue.Interfaces;
using WebUser.features.Cart.Interfaces;
using WebUser.features.Category.Interfaces;
using WebUser.features.Coupon.Interfaces;
using WebUser.features.discount.Interfaces;
using WebUser.features.Image.Interfaces;
using WebUser.features.Order.Interfaces;
using WebUser.features.OrderProduct.Interfaces;
using WebUser.features.Point.Interfaces;
using WebUser.features.Promotion.Interfaces;
using WebUser.features.User.Interfaces;
using WebUser.PricingService.interfaces;

namespace WebUser.shared.RepoWrapper
{
    public interface IServiceWrapper
    {
        Task SaveAsync();
        IAttributeNameService AttributeName { get; }
        IAttributeValueService AttributeValue { get; }
        ICartService Cart { get; }
        ICategoryService Category { get; }
        ICouponService Coupon { get; }
        IDiscountService Discount { get; }
        IImageService Image { get; }
        IOrderService Order { get; }
        IOrderProductService OrderProduct { get; }
        IPointService Point { get; }
        IProductService Product { get; }
        IPromotionService Promotion { get; }
        IUserService User { get; }
        IPricingService Pricing { get; }

    }
}
