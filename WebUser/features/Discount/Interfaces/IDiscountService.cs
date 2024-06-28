using E = WebUser.Domain.entities;

namespace WebUser.features.discount.Interfaces
{
    public interface IDiscountService
    {
        public Task<double> ApplyDiscountForProduct(E.Product Product);
        public void Update(E.Discount discount);
    }
}
