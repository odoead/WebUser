using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Coupon
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public bool IsActivated { get; set; } = false;

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ActiveFrom { get; set; }

        [Required]
        public DateTime ActiveTo { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double? DiscountVal { get; set; }

        [Range(1, 100, ErrorMessage = "Only 1-100 range allowed")]
        public int? DiscountPercent { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("OrderID")]
        public Order? Order { get; set; }
        public int? OrderID { get; set; }

        public static bool IsActive(Coupon coupon)
        {
            var isValid = coupon.ActiveFrom < DateTime.UtcNow && coupon.ActiveTo > DateTime.UtcNow && coupon.IsActivated == false;
            return isValid;
        }
    }
}
