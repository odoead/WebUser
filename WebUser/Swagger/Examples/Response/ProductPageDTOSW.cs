namespace WebUser.features.Product.DTO;

using Swashbuckle.AspNetCore.Filters;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;
using WebUser.features.Promotion_TODO.DTO;

public class ProductPageDTOSW : IExamplesProvider<ProductPageDTO>
{
    public ProductPageDTO GetExamples() =>
        new()
        {
            ID = 1,
            Description = "description",
            Name = "smartphone1",
            BasePrice = 1000,
            IsPurchasable = true,
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
            Promotions = new List<PromotionProductPageDTO>()
            {
                new() { Name = "sale", Description = "buy one get two promo" },
            },
        };
}
