namespace WebUser.features.Discount.DTO;

public class DiscountMinDTO
{
    public int ID { get; set; }
    public double? DiscountVal { get; set; }
    public int? DiscountPercent { get; set; }
    public bool IsActive { get; set; }
}
