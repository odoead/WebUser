using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Product.DTO;

namespace WebUser.features.Promotion.DTO
{
    public class PromotionDTO
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }

        //actions
        public double? DiscountVal { get; set; }
        public int? DiscountPercent { get; set; }
        public int? GetQuantity { get; set; }
        public int? PointsValue { get; set; }
        public int? PointsPercent { get; set; }
        public int? PointsExpireDays { get; set; }
        public List<ProductMinDTO> PromoProducts { get; set; } = new List<ProductMinDTO>();

        //conditions
        public double? MinPay { get; set; }
        public int? BuyQuantity { get; set; }
        public bool? IsFirstOrder { get; set; }
        public List<CategoryMinDTO> Categories { get; set; } = new List<CategoryMinDTO>();
        public List<ProductMinDTO> Products { get; set; } = new List<ProductMinDTO>();
        public List<AttributeNameValueDTO> AttributeValues { get; set; } = new List<AttributeNameValueDTO>();
    }
}
