using System.ComponentModel.DataAnnotations.Schema;
using WebUser.Domain.entities;
using WebUser.features.CartItem.DTO;
//using WebUser.Domain.entities;
using E=WebUser.Domain.entities;

namespace WebUser.features.Cart.DTO
{
    public class CartDTO
    {
        public int ID { get; set; }
        public ICollection<CartItemDTO>? items { get; set; } = null;
        public User User { get; set; } = null;
    }
}
