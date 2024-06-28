namespace WebUser.features.Product.DTO;

using WebUser.features.AttributeValue.DTO;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;
using WebUser.features.Promotion_TODO.DTO;

public class ProductPageDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double BasePrice { get; set; }
    public bool IsPurchasable { get; set; }
    public List<ImageDTO> Images { get; set; }
    public List<AttributeNameValueDTO> AttributeValues { get; set; }
    public List<DiscountMinDTO> Discounts { get; set; }
    public List<PromotionProductPageDTO> Promotions { get; set; }
}
