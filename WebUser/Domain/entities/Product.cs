using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Price { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive number and 0 allowed")]
        public int Stock { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<AttributeValue> AttributeValues { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; }

    }
}
