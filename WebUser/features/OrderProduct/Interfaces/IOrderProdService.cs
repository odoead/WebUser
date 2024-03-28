using WebUser.shared;
using E = WebUser.Domain.entities;
namespace WebUser.features.OrderProduct.Interfaces
{
    public interface IOrderProductService
    {
        public void Delete(E.OrderProduct OrderProduct);
        public Task<ICollection<E.OrderProduct>>? GetAllAsync();
        public Task<E.OrderProduct>? GetByIdAsync(ObjectID<E.OrderProduct> Id);

        public Task<ICollection<E.OrderProduct>>? GetByUserIdAsync(ObjectID<E.User> Id);
        public Task<bool> IsExistsAsync(ObjectID<E.OrderProduct> Id);

        public void Update(E.OrderProduct OrderProduct);

    }
}
