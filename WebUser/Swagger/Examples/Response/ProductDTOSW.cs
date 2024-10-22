using Swashbuckle.AspNetCore.Filters;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Coupon.DTO;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;
using WebUser.features.Promotion_TODO.DTO;

namespace WebUser.features.Product.DTO
{
    public class ProductDTOSW : IExamplesProvider<ProductDTO>
    {
        public ProductDTO GetExamples() =>
            new()
            {
                ID = 1,
                Description = "description",
                Name = "smartphone1",
                Price = 1000,
                Stock = 100,
                ReservedStock = 10,
                IsPurchasable = true,
                DateCreated = DateTime.UtcNow,
                Images = new List<ImageDTO>
                {
                    new() { ID = 1, ImageContent = new byte[] { } },
                },
                AttributeValues = new List<AttributeNameValueDTO>()
                {
                    new()
                    {
                        AttributeName = new AttributeNameMinDTO { ID = 1, Name = "Color" },
                        Attributes = new List<AttributeValueDTO>
                        {
                            new() { ID = 1, Value = "Red" },
                            new() { ID = 2, Value = "Blue" },
                        },
                    },
                },
                Discounts = new List<DiscountMinDTO>()
                {
                    new()
                    {
                        ID = 1,
                        DiscountVal = 56,
                        DiscountPercent = 0,
                        IsActive = true,
                    },
                },
                Coupons = new List<CouponMinDTO>()
                {
                    new()
                    {
                        ID = 1,
                        DiscountVal = 100,
                        DiscountPercent = 5,
                        IsActive = true,
                    },
                },
                Promotions = new List<PromotionMinDTO>()
                {
                    new() { ID = 1, Name = "sale" },
                },
            };
    }
}
