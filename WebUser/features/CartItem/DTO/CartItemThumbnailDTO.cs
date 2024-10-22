namespace WebUser.features.CartItem.DTO;

using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;

public class CartItemThumbnailDTO
{
    public int ID { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public double BasePrice { get; set; }
    public List<DiscountMinDTO> Discounts { get; set; }
    public bool IsPurchasable { get; set; }
    public List<ImageDTO> Images { get; set; }
    public int CartItemStock { get; set; }
    public int TotalFreeStock { get; set; }
}
