using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.Category.Interfaces
{
    public interface ICategoryService
    {
        public Task<E.Category> GetByIdAsync(ObjectID<E.Category> Id);
        Task<ICollection<E.Category>> GetAllAsync();
        public void Delete(E.Category category);
        public void Update(E.Category category);
        public void Create(E.Category category);
        public  Task<bool> IsExistsAsync(ObjectID<E.Category> Id);
        public IEnumerable<E.Category> ShowAllChildCategories(E.Category category);
        public IEnumerable<E.Category> ShowFirstChildCategories(E.Category category);
        public void AddAttributeName(E.AttributeName attributeName, E.Category category);
        public void RemoveAttributeName(E.AttributeName attributeName, E.Category category);

    }
}
