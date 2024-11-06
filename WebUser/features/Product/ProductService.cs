using WebUser.Data;
using WebUser.features.Cart.Interfaces;
using WebUser.PricingService.DTO;

namespace WebUser.features.Cart
{
    public class ProductService : IProductService
    {
        private readonly DB_Context dbcontext;

        public ProductService(DB_Context context)
        {
            dbcontext = context;
        }

        public double CalculatePriceWithCumulativeDiscounts(int productId, double basePrice, List<DiscountRecordDTO> appliedDiscounts)
        {
            if (appliedDiscounts.Any())
            {
                return 0;
            }
            var productDiscounts = appliedDiscounts.Where(d => d.ProductId == productId).ToList();

            double totalAbsoluteDiscount = productDiscounts
                .Where(d => d.ValueTypes.Contains(DiscountValueType.Absolute))
                .Sum(d => d.AbsoluteDiscountValue ?? 0);

            double totalPercentageDiscount = productDiscounts
                .Where(d => d.ValueTypes.Contains(DiscountValueType.Percentage))
                .Sum(d => d.PercentDiscountValue ?? 0);

            double price = basePrice - totalAbsoluteDiscount - totalPercentageDiscount;

            return price;
        }
    }
}
