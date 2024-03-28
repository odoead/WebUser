using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebUser.Domain.entities
{
    public class OrderProduct
    {
        [Key]
        public int ID { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed,excluding 0")]
        [Required]
        public int Amount { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int? ProductId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public int? OrderId { get; set; }
    }
}
