using WebUser.shared;
using E=WebUser.Domain.entities;
namespace WebUser.features.Cart.Interfaces
{
    public interface ICartItemService
    {
        Task<E.CartItem> GetByIdAsync(ObjectID<E.CartItem> id);
        Task<ICollection<E.CartItem>> GetAllAsync();
        public void Delete(E.CartItem cart);
        public void Update(E.CartItem cart);
        public void Create(E.CartItem cart);
        public Task<bool> IsExistsAsync(ObjectID<E.CartItem> id);
        public Task<ICollection<E.CartItem>>? GetByCartIdAsync(ObjectID<E.Cart> id);


    }
}
