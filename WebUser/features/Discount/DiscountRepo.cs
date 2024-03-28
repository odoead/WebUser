using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.discount.Interfaces;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.discount
{
    public class DiscountRepo : IDiscountService
    {
        private DB_Context _Context;
        public DiscountRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.Discount discount)
        {
            _Context.Add(discount);
        }

        public void Delete(E.Discount discount)
        {
            _Context.discounts.Remove(discount);
        }

        public async Task<ICollection<E.Discount>>? GetAllAsync()
        {
            return await _Context.discounts.ToListAsync();
        }

        public async Task<E.Discount>? GetByIdAsync(ObjectID<E.Discount> Id)
        {
            return await _Context.discounts.Where(q => q.ID == Id.Value).FirstOrDefaultAsync();
        }
        public async Task<E.Discount>? GetByProductIdAsync(ObjectID<E.Product> Id)
        {
            return await _Context.discounts.Where(q => q.Product.ID == Id.Value).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(ObjectID<E.Discount> Id)
        {
            return await _Context.discounts.AnyAsync(q => q.ID == Id.Value);
        }
        public void Update(E.Discount discount)
        {
            _Context.discounts.Update(discount);
        }
    }
}
