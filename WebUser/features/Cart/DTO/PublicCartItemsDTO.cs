namespace WebUser.features.Cart.DTO;

using WebUser.features.CartItem.DTO;

public class PublicCartItemsDTO
{
    public List<CartItemThumbnailDTO> CartItems { get; set; }
    public double TotalCost { get; set; }
}

