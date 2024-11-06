namespace WebUser.features.Cart.DTO;

using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;

public class CartItemThumbnailDTO
{
    public int ID { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public double BasePrice { get; set; }
    public double FinalSinglePrice { get; set; }

    public PublicDiscountDTO Discount { get; set; }
    public bool IsPurchasable { get; set; }
    public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    public int CartItemStock { get; set; }
    public int TotalFreeStock { get; set; }
}
