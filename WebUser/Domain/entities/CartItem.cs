
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class CartItem
    {
        [Key]
        public int ID { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        [Required]
        public int Amount { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        public int? CartId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
