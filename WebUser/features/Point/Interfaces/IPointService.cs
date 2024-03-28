using E=WebUser.Domain.entities;
namespace WebUser.features.Point.Interfaces
{
    public interface IPointService
    {
        Task<E.Point> GetByIdAsync(int id);
        Task<ICollection<E.Point>> GetAllAsync();
        public void Delete(E.Point Point);
        public void Update(E.Point Point);
        public void Create(E.Point Point);
        public Task<bool> IsExistsAsync(int id);
    }
}
