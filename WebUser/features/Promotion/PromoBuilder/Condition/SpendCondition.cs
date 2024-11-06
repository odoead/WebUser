using WebUser.PricingService.DTO;

namespace WebUser.features.Promotion.PromoBuilder.Condition
{
    public class SpendCondition : PromotionCondition<ProductCalculationDTO>
    {
        public SpendCondition(double requiredTotal)
            : base(cart => cart.Sum(p => CalculateFinalPrice(p) * p.Quantity) >= requiredTotal) { }

        public static double CalculateFinalPrice(ProductCalculationDTO product)
        {
            double absoluteDiscounts = product
                .AppliedDiscounts.Where(d => d.AbsoluteDiscountValue.HasValue && d.AbsoluteDiscountValue > 0)
                .Sum(d => d.AbsoluteDiscountValue.Value);

            double percentDiscounts = product
                .AppliedDiscounts.Where(d => d.PercentDiscountValue.HasValue && d.PercentDiscountValue > 0)
                .Sum(d => d.PercentDiscountValue.Value);

            double finalPrice = product.BasePrice - absoluteDiscounts - percentDiscounts;

            return finalPrice > 0 ? finalPrice : 0;
        }
    }

    public static class SpendConditionExtention
    {
        public static bool IsPriceBiggerThan(this ICollection<ProductCalculationDTO> productCalculations, double requiredTotal)
        {
            var specification = new SpendCondition(requiredTotal);
            return specification.ApplyRule(productCalculations);
        }

        public static bool IsPriceBiggerThan(this IQueryable<ProductCalculationDTO> productCalculations, double requiredTotal)
        {
            var specification = new SpendCondition(requiredTotal);
            return specification.ApplyRule(productCalculations);
        }

        public static IQueryable<ProductCalculationDTO> PriceBiggerThan(this IQueryable<ProductCalculationDTO> productCalculations, double requiredTotal
        )
        {
            var specification = new SpendCondition(requiredTotal);
            return productCalculations.Where(specification.Expression);
        }
    }
}
