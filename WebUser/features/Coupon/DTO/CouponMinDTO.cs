namespace WebUser.features.Coupon.DTO;

public class CouponMinDTO
{
    public int ID { get; set; }
    public double? DiscountVal { get; set; }
    public int? DiscountPercent { get; set; }
    public bool IsActive { get; set; }
}
