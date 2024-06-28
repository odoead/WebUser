namespace WebUser.features.Product.DTO;

using Swashbuckle.AspNetCore.Filters;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;

public class ProductThumbnailDTOSW : IExamplesProvider<ProductThumbnailDTO>
{
    public ProductThumbnailDTO GetExamples() =>
        new ProductThumbnailDTO
        {
            ID = 1,
            Name = "smartphone1",
            BasePrice = 1500,
            IsPurchasable = true,
            Images = new List<ImageDTO>
            {
                new ImageDTO { ID = 1, ImageContent = new byte[] { } }
            },
            Discounts = new List<DiscountMinDTO>()
            {
                new DiscountMinDTO
                {
                    ID = 1,
                    DiscountVal = 56,
                    DiscountPercent = 0,
                    IsActive = true
                }
            },
        };
}