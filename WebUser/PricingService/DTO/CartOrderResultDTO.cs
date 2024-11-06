namespace WebUser.PricingService.DTO;

public class CartOrderResultDTO
{
    public int UsedBonusPoints { get; set; }
    public double OrderCost { get; set; }
    public List<ProductCalculationDTO> Products { get; set; } = new List<ProductCalculationDTO>();
}
