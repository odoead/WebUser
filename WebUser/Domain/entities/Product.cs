using System.ComponentModel.DataAnnotations;

namespace WebUser.Domain.entities
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive number and 0 allowed")]
        public int Stock { get; set; }
        public int ReservedStock { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<ProductAttributeValue> AttributeValues { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; }
        public ICollection<Discount> Discounts { get; set; }
        public ICollection<PromotionProduct> Promotions { get; set; }
        public ICollection<PromotionPromProduct> PromoPromotions { get; set; }
        public ICollection<Coupon> Coupons { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
