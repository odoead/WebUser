namespace WebUser.PricingService.DTO;

public class ProductCalculationDTO
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double BasePrice { get; set; }
    public double FinalSinglePrice { get; set; }
    public List<DiscountRecordDTO> AppliedDiscounts { get; set; } = new List<DiscountRecordDTO>();
    public bool IsPurchasable { get; set; }
    public List<int> ActivatedCoupons { get; set; }
}
