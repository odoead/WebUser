namespace Test.mocks;

using System.Threading.Tasks;
using Moq;
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
using WebUser.shared.RepoWrapper;

internal class ServiceWrapperMock
{
    public static Mock<IServiceWrapper> CreateMock()
    {
        var mockServiceWrapper = new Mock<IServiceWrapper>();

        mockServiceWrapper.Setup(service => service.SaveAsync()).Returns(Task.CompletedTask);

        mockServiceWrapper.Setup(service => service.AttributeName).Returns(Mock.Of<IAttributeNameService>());

        mockServiceWrapper.Setup(service => service.AttributeValue).Returns(Mock.Of<IAttributeValueService>());

        mockServiceWrapper.Setup(service => service.Cart).Returns(Mock.Of<ICartService>());

        mockServiceWrapper.Setup(service => service.Category).Returns(Mock.Of<ICategoryService>());

        mockServiceWrapper.Setup(service => service.Coupon).Returns(Mock.Of<ICouponService>());

        mockServiceWrapper.Setup(service => service.Discount).Returns(Mock.Of<IDiscountService>());

        mockServiceWrapper.Setup(service => service.Image).Returns(Mock.Of<IImageService>());

        mockServiceWrapper.Setup(service => service.Order).Returns(Mock.Of<IOrderService>());

        mockServiceWrapper.Setup(service => service.OrderProduct).Returns(Mock.Of<IOrderProductService>());

        mockServiceWrapper.Setup(service => service.Point).Returns(Mock.Of<IPointService>());

        mockServiceWrapper.Setup(service => service.Product).Returns(Mock.Of<IProductService>());

        mockServiceWrapper.Setup(service => service.Promotion).Returns(Mock.Of<IPromotionService>());

        mockServiceWrapper.Setup(service => service.User).Returns(Mock.Of<IUserService>());

        mockServiceWrapper.Setup(service => service.Pricing).Returns(Mock.Of<IPricingService>());

        return mockServiceWrapper;
    }
}
