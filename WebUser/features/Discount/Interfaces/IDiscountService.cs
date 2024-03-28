using WebUser.shared;
using E = WebUser.Domain.entities;
namespace WebUser.features.discount.Interfaces
{
    public interface IDiscountService
    {
        /*public void CreateWithPercentDiscount(DateTime ActiveFrom, DateTime ActiveTo, float discount);
        public void CreateWithNumberDiscount(DateTime ActiveFrom, DateTime ActiveTo, double discount);*/
        public void Create(E.Discount discount);

        public void Delete(E.Discount discount);

        public Task<ICollection<E.Discount>> GetAllAsync();

        public Task<E.Discount> GetByIdAsync(ObjectID<E.Discount> Id);

        public Task<E.Discount> GetByProductIdAsync(ObjectID<E.Product> Id);
        public Task<bool> IsExistsAsync(ObjectID<E.Discount> Id);

        public void Update(E.Discount discount);

    }
}
