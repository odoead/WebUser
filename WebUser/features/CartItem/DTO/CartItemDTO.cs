using WebUser.features.Product.DTO;

namespace WebUser.features.CartItem.DTO
{
    public class CartItemDTO
    {
        public int ID { get; set; }
        public int Amount { get; set; }
        public ProductMinDTO ProductMin { get; set; }
    }
}
