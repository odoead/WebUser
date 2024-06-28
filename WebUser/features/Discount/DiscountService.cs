using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.discount.Interfaces;
using E = WebUser.Domain.entities;

namespace WebUser.features.discount
{
    public class DiscountService : IDiscountService
    {
        private readonly DB_Context Context;

        public DiscountService(DB_Context context)
        {
            Context = context;
        }

        public async Task<double> ApplyDiscountForProduct(E.Product Product)
        {
            var discount = await Context
                .Discounts.Where(q => q.Product.ID == Product.ID && q.ActiveFrom <= DateTime.UtcNow && q.ActiveTo >= DateTime.UtcNow)
                .FirstOrDefaultAsync();
            var newPrice = discount.Product.Price;
            if (discount.DiscountVal > 0)
            {
                newPrice -= discount.DiscountVal;
            }
            if (discount.DiscountPercent > 0)
            {
                newPrice = newPrice * discount.DiscountVal / 100;
            }
            return newPrice;
        }

        public void Update(E.Discount discount) => Context.Discounts.Update(discount);
    }
}
