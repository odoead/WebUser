using System.ComponentModel.DataAnnotations;

namespace WebUser.Domain.entities
{
    public class Promotion
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ActiveFrom { get; set; }

        [Required]
        public DateTime ActiveTo { get; set; }

        //conditions
        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double? MinPay { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int? BuyQuantity { get; set; }
        public bool? IsFirstOrder { get; set; }
        public List<PromotionCategory> Categories { get; set; } = new List<PromotionCategory> { };
        public List<PromotionProduct> ConditionProducts { get; set; } = new List<PromotionProduct> { };
        public List<PromotionAttrValue> AttributeValues { get; set; } = new List<PromotionAttrValue> { };

        //actions
        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double? DiscountVal { get; set; }

        [Range(1, 100, ErrorMessage = "Only 1-100 range allowed")]
        public int? DiscountPercent { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int? GetQuantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int? PointsValue { get; set; }

        [Range(1, 100, ErrorMessage = "Only 1-100 range allowed")]
        public int? PointsPercent { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int? PointsExpireDays { get; set; }
        public List<PromotionPromProduct> ActionProducts { get; set; } = new List<PromotionPromProduct> { };
        public static bool IsActive(Promotion promotion)
        {
            var isValid = promotion.ActiveFrom < DateTime.UtcNow && promotion.ActiveTo > DateTime.UtcNow;
            return isValid;
        }
    }
}
