using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using E=WebUser.Domain.entities;
using WebUser.features.Cart.Interfaces;
using WebUser.features.Promotion.Interfaces;

namespace WebUser.features.Promotion
{
    public class PromotionRepo : IPromotionService
    {
        private DB_Context _Context;
        public PromotionRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.Promotion Promotion)
        {
            _Context.promotions.Add(Promotion);
        }

        public void Delete(E.Promotion Promotion)
        {
            _Context.promotions.Remove(Promotion);
        }

        public async Task<ICollection<E.Promotion>>? GetAllAsync()
        {
            return await _Context.promotions.ToListAsync();
        }

        public async Task<E.Promotion>? GetByIdAsync(int id)
        {
            return await _Context.promotions.Where(q => q.ID == id).FirstOrDefaultAsync();
        }

        public async Task< bool> IsExistsAsync(int id)
        {
            return await _Context.promotions.AnyAsync(q=>q.ID == id);
        }
        public void Update(E.Promotion Promotion)
        {
            _Context.promotions.Update(Promotion);
        }
    }
}
