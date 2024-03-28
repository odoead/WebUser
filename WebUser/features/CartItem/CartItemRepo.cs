using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.Interfaces;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class CartItemRepo : ICartItemService
    {
        private DB_Context _Context;
        public CartItemRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.CartItem cartItem)
        {
            _Context.cartItems.Add(cartItem);
        }

        public void Delete(E.CartItem cartItem)
        {
            _Context.cartItems.Remove(cartItem);
        }

        public async Task<ICollection<E.CartItem>>? GetAllAsync()
        {
            return await _Context.cartItems.ToListAsync();
        }

        public async Task<E.CartItem>? GetByIdAsync(ObjectID<E.CartItem> id)
        {

            return await _Context.cartItems.Where(q => q.ID == id.Value).FirstOrDefaultAsync();

        }
        public async Task<ICollection<E.CartItem>>? GetByCartIdAsync(ObjectID<E.Cart> id)
        {

            return await _Context.cartItems.Where(q => q.Cart.ID == id.Value).ToListAsync();

        }

        public async Task<bool> IsExistsAsync(ObjectID<E.CartItem> id)
        {
            return await _Context.cartItems.AnyAsync(q => q.ID == id.Value);
        }
        public void Update(E.CartItem cart)
        {
            _Context.cartItems.Update(cart);
        }
       
    }
}
