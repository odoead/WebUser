using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Discount
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ActiveFrom { get; set; }

        [Required]
        public DateTime ActiveTo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double? DiscountVal { get; set; }

        [Range(1, 100, ErrorMessage = "Only 1-100 range allowed")]
        public int? DiscountPercent { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public int ProductID { get; set; }

        public static bool IsActive(Discount discount)
        {
            var isValid = discount.ActiveFrom <DateTime.UtcNow&&discount.ActiveTo>DateTime.UtcNow;
            return isValid;
        }
    }
}
