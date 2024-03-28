using E=WebUser.Domain.entities;

namespace WebUser.features.Cart.DTO
{
    public class CartUpdateDTO
    {
        public ICollection<E.CartItem> items { get; set; }
        public E.User User { get; set; }
    }
}
