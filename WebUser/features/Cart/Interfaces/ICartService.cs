using WebUser.Domain.entities;
using WebUser.shared;
using E=WebUser.Domain.entities;
namespace WebUser.features.Cart.Interfaces
{
    public interface ICartService
    {
        public Task<E.Cart> GetByIdAsync(ObjectID<E.Cart> Id);
        public Task<E.Cart> GetByUserIdAsync(ObjectID<E.User> Id);

        public void Create(E.Cart Cart);
        public Task<bool> IsExistsAsync(ObjectID<E.Cart> Id);
        Task<ICollection<E.Cart>> GetAllAsync();
        public void Delete(E.Cart cart);
        public void Update(E.Cart cart);
       

    }
}
