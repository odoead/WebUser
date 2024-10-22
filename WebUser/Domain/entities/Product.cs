using System.ComponentModel.DataAnnotations;

namespace WebUser.Domain.entities
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }

        //[Column(TypeName = "Date")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive number and 0 allowed")]
        public int Stock { get; set; }
        public int ReservedStock { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<ProductAttributeValue> AttributeValues { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; } = new List<OrderProduct>();
        public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
        public ICollection<PromotionProduct> Promotions { get; set; } = new List<PromotionProduct>();
        public ICollection<PromotionPromProduct> PromoPromotions { get; set; } = new List<PromotionPromProduct>();
        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductUserNotificationRequest> RequestNotifications { get; set; }
    }
}
