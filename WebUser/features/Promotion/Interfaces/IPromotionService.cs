using E=WebUser.Domain.entities;
namespace WebUser.features.Promotion.Interfaces
{
    public interface IPromotionService
    {
        Task<E.Promotion> GetByIdAsync(int id);
        Task<ICollection<E.Promotion>> GetAllAsync();
        public void Delete(E.Promotion Promotion);
        public void Update(E.Promotion Promotion);
        public void Create(E.Promotion Promotion);
        public Task<bool> IsExistsAsync(int id);
    }
}
