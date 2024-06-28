using E = WebUser.Domain.entities;

namespace WebUser.features.Category.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<E.Category>> ShowAllChildCategories(int categoryId);
        public IEnumerable<E.Category> ShowFirstChildCategories(int categoryId);
    }
}
