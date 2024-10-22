using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        [ForeignKey("UserID")]
        public User User { get; set; }
        public string UserID { get; set; }
    }
}
