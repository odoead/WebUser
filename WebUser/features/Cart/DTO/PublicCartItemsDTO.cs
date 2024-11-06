namespace WebUser.features.Cart.DTO;
public class PublicCartItemsDTO
{
    public List<CartItemThumbnailDTO> CartItems { get; set; } = new List<CartItemThumbnailDTO>();
    public double TotalCost { get; set; }
}

