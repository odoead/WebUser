using E=WebUser.Domain.entities;

namespace WebUser.features.CartItem.DTO
{
    public class CartItemUpdateDTO
    {
        public E.Cart Cart { get; set; }
        public int Amount { get; set; }
    }
}
