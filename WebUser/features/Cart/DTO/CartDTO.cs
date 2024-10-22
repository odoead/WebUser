using WebUser.features.CartItem.DTO;

namespace WebUser.features.Cart.DTO
{
    public class CartDTO
    {
        public int ID { get; set; }
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public string UserId { get; set; } = "";
    }
}
