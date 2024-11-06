using WebUser.PricingService.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.PromoBuilder.Actions
{
    public static class GetFreeItem
    {


        public static List<ProductCalculationDTO> GetFreeItemAct(this ICollection<E.Product> products, IEnumerable<E.Product> freeProducts, int quantity)
        {
            return GenerateFreeItems(products, freeProducts, quantity);
        }

        public static List<ProductCalculationDTO> GetFreeItemAct(this IQueryable<E.Product> products, IEnumerable<E.Product> promProducts, int quantity)
        {
            return GenerateFreeItems(products, promProducts, quantity);
        }
        private static List<ProductCalculationDTO> GenerateFreeItems(
            IEnumerable<E.Product> products,
            IEnumerable<E.Product> promProducts,
            int quantity
        )
        {
            var discountRecords = new List<ProductCalculationDTO>();

            foreach (var product in products)
            {
                if (promProducts.Any(p => p.ID == product.ID))
                {
                    var freeItem = new ProductCalculationDTO
                    {
                        ProductId = product.ID,
                        Name = product.Name,
                        Quantity = quantity,
                        BasePrice = product.Price * quantity,
                        FinalSinglePrice = 0,
                        IsPurchasable = E.Product.IsPurchasable(product, quantity),
                        AppliedDiscounts = new List<DiscountRecordDTO>(),
                    };

                    freeItem.AppliedDiscounts.Add(
                        new DiscountRecordDTO
                        {
                            ProductId = product.ID,
                            Type = DiscountType.PromotionDiscount,
                            ValueTypes = new List<DiscountValueType> { DiscountValueType.Percentage },
                            Percent = 100,
                            PercentDiscountValue = product.Price, // 100% discount
                        }
                    );

                    discountRecords.Add(freeItem);
                }
            }

            return discountRecords;
        }
    }
}
