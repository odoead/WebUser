using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.Interfaces;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class ProductRepo : IProductService
    {
        private DB_Context _Context;
        public ProductRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.Product product)
        {
            _Context.products.Add(product);
        }
        public void Delete(E.Product product)
        {
            _Context.products.Remove(product);
        }

        public async Task<ICollection<E.Product>>? GetAllAsync()
        {
            return await _Context.products.ToListAsync();
        }

        public async Task<E.Product>? GetByIdAsync(ObjectID<E.Product> Id)
        {
            return await _Context.products.Where(q => q.ID == Id.Value).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(ObjectID<E.Product> Id)
        {
            return await _Context.products.AnyAsync(q => q.ID == Id.Value);
        }

        public void Update(E.Product product)
        {
            _Context.products.Update(product);
        }
    }
}
