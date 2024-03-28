using WebUser.shared;
using E = WebUser.Domain.entities;
namespace WebUser.features.Image.Interfaces
{
    public interface IImageService
    {
        Task<ICollection<E.Image>> GetAllAsync();
        public void Delete(E.Image discount);
        public void Update(E.Image discount);
        public void Create(E.Image discount);
        public Task<bool> IsExistsAsync(ObjectID<E.Image> Id);
        public Task<E.Image> GetByIdAsync(ObjectID<E.Image> Id);
        public Task<ICollection<E.Image>> GetByProductIdAsync(ObjectID<E.Product> Id);
    }
}
