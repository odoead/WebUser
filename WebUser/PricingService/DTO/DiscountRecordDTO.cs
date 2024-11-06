namespace WebUser.PricingService.DTO;

public class DiscountRecordDTO
{
    public int ProductId { get; set; }
    public DiscountType Type { get; set; }
    public List<DiscountValueType> ValueTypes { get; set; } = new List<DiscountValueType>();
    public int? Percent { get; set; }
    public double? PercentDiscountValue { get; set; }

    public double? AbsoluteDiscountValue { get; set; }
}

public enum DiscountType
{
    ProductDiscount,
    CouponDiscount,
    PromotionDiscount,
    BonusPointsDiscount,
}

public enum DiscountValueType
{
    Percentage,
    Absolute,
}
