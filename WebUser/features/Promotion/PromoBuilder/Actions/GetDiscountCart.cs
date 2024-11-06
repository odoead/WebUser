using WebUser.PricingService.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Actions
{
    public static class GetDiscountCart
    {

        #region new 
        public static ICollection<DiscountRecordDTO> GetDiscountAct(this List<E.Product> products, double discountValue, int discountPercent) =>
            GenerateDiscounts(products, discountValue, discountPercent);

        public static ICollection<DiscountRecordDTO> GetDiscountAct(this IQueryable<E.Product> productsQuery, double discountValue, int discountPercent
        ) => GenerateDiscounts(productsQuery.ToList(), discountValue, discountPercent);

        /// <summary>
        /// generate discount objects for order/cart
        /// </summary>
        /// <param name="products"></param>
        /// <param name="discountValue"></param>
        /// <param name="discountPercent"></param>
        /// <returns></returns>
        private static ICollection<DiscountRecordDTO> GenerateDiscounts(List<E.Product> products, double discountValue, int discountPercent)
        {
            var discountRecords = new List<DiscountRecordDTO>();
            foreach (var product in products)
            {
                var basePrice = product.Price;
                var record = new DiscountRecordDTO { ProductId = product.ID, Type = DiscountType.PromotionDiscount, ValueTypes = new() };
                if (discountValue > 0)
                {
                    record.AbsoluteDiscountValue = discountValue;
                    record.ValueTypes.Add(DiscountValueType.Absolute);
                }
                if (discountPercent > 0)
                {
                    record.PercentDiscountValue = Math.Floor(basePrice * discountPercent / 100);
                    record.Percent = discountPercent;
                    record.ValueTypes.Add(DiscountValueType.Percentage);
                }
                discountRecords.Add(record);
            }
            return discountRecords;
        }
        #endregion
    }
}
