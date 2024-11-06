namespace WebUser.features.Cart.Interfaces
{
    using WebUser.PricingService.DTO;

    public interface IProductService
    {

        public double CalculatePriceWithCumulativeDiscounts(int productId, double basePrice, List<DiscountRecordDTO> appliedDiscounts);
    }
}
