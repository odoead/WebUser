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
        public double MinPay { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int BuyQuantity { get; set; }
        public bool IsFirstOrder { get; set; }
        public ICollection<PromotionCategory> Categories { get; set; }
        public ICollection<PromotionProduct> Products { get; set; }
        public ICollection<PromotionAttrValue> AttributeValues { get; set; }

        //actions
        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double DiscountVal { get; set; }

        [Range(1, 100, ErrorMessage = "Only 1-100 range allowed")]
        public int DiscountPercent { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int GetQuantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int PointsValue { get; set; }

        [Range(1, 100, ErrorMessage = "Only 1-100 range allowed")]
        public int PointsPercent { get; set; }
        public int PointsExpireDays { get; set; }
        public ICollection<PromotionPromProduct> PromoProducts { get; set; }
    }
}
