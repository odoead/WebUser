namespace WebUser.features.Product.DTO;

using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;

public class ProductThumbnailDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public double BasePrice { get; set; }
    public double AfterDiscountPrice { get; set; }

    public List<DiscountMinDTO> Discounts { get; set; } = new List<DiscountMinDTO>();
    public bool IsPurchasable { get; set; }
    public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
}
