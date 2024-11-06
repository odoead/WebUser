namespace WebUser.features.Promotion.PromoBuilder.Actions
{
    using System.Collections.Generic;
    using WebUser.PricingService.DTO;
    using E = WebUser.Domain.entities;

    public static class GetDiscountForItems
    {

        public static List<DiscountRecordDTO> GetDiscountForItemtAct(
            this ICollection<E.Product> products,
            IEnumerable<E.Product> promProducts,
            double discountValue = 0,
            int discountPercent = 0
        )
        {
            return GenerateDiscounts(products, promProducts, discountValue, discountPercent);
        }

        public static List<DiscountRecordDTO> GetDiscountForItemtAct(
            this IQueryable<E.Product> products,
            IEnumerable<E.Product> promProducts,
            double discountValue = 0,
            int discountPercent = 0
        )
        {
            return GenerateDiscounts(products, promProducts, discountValue, discountPercent);
        }

        /// <summary>
        /// generate discount objects for order/cart products
        /// </summary>
        /// <param name="products">representation of cart/order products</param>
        /// <param name="promProducts">list of promoting products</param>
        /// <param name="discountValue"></param>
        /// <param name="discountPercent"></param>
        /// <returns></returns>
        private static List<DiscountRecordDTO> GenerateDiscounts(IEnumerable<E.Product> products, IEnumerable<E.Product> promProducts,
            double discountValue, int discountPercent)
        {
            var discountRecords = new List<DiscountRecordDTO>();
            foreach (var product in products)
            {
                if (promProducts.Select(q => q.ID).Contains(product.ID))
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
            }
            return discountRecords;
        }
    }
}
