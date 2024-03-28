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
        public double DiscountVal { get; set; }
        [Range(1, 100, ErrorMessage = "Only 1-100 range allowed")]
        public float DiscountPercent { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int? ProductId { get; set; }
    }
}
