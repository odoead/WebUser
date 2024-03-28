using WebUser.shared;
using E = WebUser.Domain.entities;
namespace WebUser.features.Cart.Interfaces
{
    public interface IProductService
    {
        public void Create(E.Product product);


        public void Delete(E.Product product);

        public Task<ICollection<E.Product>> GetAllAsync();

        public Task<E.Product> GetByIdAsync(ObjectID<E.Product> Id);
        public Task<bool> IsExistsAsync(ObjectID<E.Product> Id);
        public void Update(E.Product product);

    }
}
