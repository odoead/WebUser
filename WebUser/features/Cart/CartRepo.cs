using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.Exceptions;
using WebUser.features.Cart.Interfaces;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class CartRepo : ICartService
    {
        private DB_Context _Context;
        public CartRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.Cart Cart)
        {
            _Context.carts.Add(Cart);
        }


        public void Delete(E.Cart cart)
        {
            _Context.carts.Remove(cart);
        }

        public async Task<ICollection<E.Cart>>? GetAllAsync()
        {
            return await _Context.carts.ToListAsync();
        }

        public async Task<E.Cart>? GetByIdAsync(ObjectID<E.Cart> Id)
        {
            if (await _Context.carts.AnyAsync(q => q.ID == Id.Value))
            {
                return await _Context.carts.Where(q => q.ID == Id.Value).FirstOrDefaultAsync();
            }
            else
            {
                throw new CartNotFoundException(Id.Value);
            }
        }
        public async Task<E.Cart>? GetByUserIdAsync(ObjectID<E.User> Id)
        {
            return await _Context.carts.Where(q => q.User.Id == Id.Value).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(ObjectID<E.Cart> Id)
        {
            return await _Context.carts.AnyAsync(q => q.ID == Id.Value);
        }

        public void Update(E.Cart cart)
        {
            _Context.carts.Update(cart);
        }
        

    }
}
