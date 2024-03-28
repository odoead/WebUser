using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double DiscountVal { get; set; }
        [Range(0.01, 100, ErrorMessage = "Only 0.01-100 range allowed")]
        public float DiscountPercent { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<AttributeName> AttributeNames { get; set; }
    }
}
