using Swashbuckle.AspNetCore.Filters;
using WebUser.features.Category.DTO;
using WebUser.features.Product.DTO;

namespace WebUser.features.Promotion.DTO
{
    public class PromotionDTOSW : IExamplesProvider<PromotionDTO>
    {
        public PromotionDTO GetExamples() =>
            new PromotionDTO
            {
                ID = 1,
                IsActive = true,
                Name = "PromotionName",
                Description = "PromotionDescription",
                CreatedAt = DateTime.UtcNow,
                ActiveFrom = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(30),
                DiscountVal = 5.0,
                DiscountPercent = 10,
                GetQuantity = 2,
                PointsValue = 50,
                PointsPercent = 20,
                PointsExpireDays = 90,
                PromoProducts = new List<ProductMinDTO>
                {
                    new ProductMinDTO
                    {
                        ID = 1,
                        Name = "Product1",
                        Price = 10.0
                    },
                    new ProductMinDTO
                    {
                        ID = 2,
                        Name = "Product2",
                        Price = 20.0
                    }
                },
                MinPay = 50.0,
                BuyQuantity = 3,
                IsFirstOrder = false,
                Categories = new List<CategoryMinDTO>
                {
                    new CategoryMinDTO { ID = 1, Name = "height" },
                    new CategoryMinDTO { ID = 2, Name = "width" }
                },
                Products = new List<ProductMinDTO>
                {
                    new ProductMinDTO
                    {
                        ID = 1,
                        Name = "Product1",
                        Price = 10.0
                    },
                    new ProductMinDTO
                    {
                        ID = 2,
                        Name = "Product2",
                        Price = 20.0
                    }
                }
            };
    }
}
