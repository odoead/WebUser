namespace WebUser.features.Promotion_TODO.DTO;

using System.ComponentModel.DataAnnotations;

public class UpdatePromotionDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ActiveFrom { get; set; }
    public DateTime ActiveTo { get; set; }
    public List<int> CategoriesIds { get; set; }
    public List<int> AttributeValueIds { get; set; }
    public List<int> ProductsForPromotionIds { get; set; }
    public List<int> ProductsForPromotionPromids { get; set; }
    public bool? IsFirstOrder { get; set; }

    public double? DiscountVal { get; set; }

    [Range(0.01, 100, ErrorMessage = "Only 0.01-100 range allowed")]
    public int? DiscountPercent { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
    public int? BuyQuantity { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
    public int? GetQuantity { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
    public double? MinPay { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
    public int? PointsValue { get; set; }

    [Range(1, 100, ErrorMessage = "Only 0.01-100 range allowed")]
    public int? PointsPercent { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
    public int? PointsExpireDays { get; set; }
}
