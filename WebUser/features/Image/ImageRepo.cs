using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Image.Interfaces;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.Image
{
    public class ImageRepo : IImageService
    {
        private DB_Context _Context;
        public ImageRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.Image image)
        {
            _Context.image.Add(image);
        }

        public void Delete(E.Image image)
        {
            _Context.image.Remove(image);
        }

        public async Task<ICollection<E.Image>>? GetAllAsync()
        {
            return await _Context.image.ToListAsync();
        }

        public async Task<E.Image>? GetByIdAsync(ObjectID<E.Image> Id)
        {
            return await _Context.image.Where(q => q.ID == Id.Value).FirstOrDefaultAsync();
        }
        public async Task<ICollection<E.Image>>? GetByProductIdAsync(ObjectID<E.Product> Id)
        {
            return await _Context.image.Where(q => q.Product.ID == Id.Value).ToListAsync();
        }
        public async Task<bool> IsExistsAsync(ObjectID<E.Image> Id)
        {
            return await _Context.image.AnyAsync(q => q.ID == Id.Value);
        }
        public void Update(E.Image image)
        {
            _Context.image.Update(image);
        }
    }
}
