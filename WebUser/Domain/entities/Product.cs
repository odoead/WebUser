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
        public List<Image> Images { get; set; } = new List<Image> { };
        public List<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue> { };
        public List<OrderProduct> OrderProduct { get; set; } = new List<OrderProduct> { };
        public List<Discount> Discounts { get; set; } = new List<Discount> { };
        public List<PromotionProduct> Promotions { get; set; } = new List<PromotionProduct> { };
        public List<PromotionPromProduct> PromoPromotions { get; set; } = new List<PromotionPromProduct> { };
        public List<Coupon> Coupons { get; set; } = new List<Coupon> { };
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<Review> Reviews { get; set; } = new List<Review> { };
        public List<ProductUserNotificationRequest> RequestNotifications { get; set; } = new List<ProductUserNotificationRequest> { };

        public static bool IsPurchasable(Product product, int quantity)
        {
            var isAvailiable =
                product.Stock > 0 && product.Stock > product.ReservedStock && product.Stock - product.ReservedStock > quantity && quantity > 0;
            return isAvailiable;
        }
    }
}
