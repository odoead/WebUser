using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        public ICollection<CartItem> items { get; set; } = new List<CartItem>();
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int? UserId { get; set; }
    }
}
