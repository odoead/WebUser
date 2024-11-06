using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        public List<CartItem> Items { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
        public string UserID { get; set; }
    }
}
