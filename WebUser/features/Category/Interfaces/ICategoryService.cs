using E = WebUser.Domain.entities;

namespace WebUser.features.Category.Interfaces
{
    public interface ICategoryService
    {
        public IEnumerable<E.Category> GetFirstGenChildCategories(int parentCategoryId);

        public Task<IEnumerable<E.Category>> GetAllGenChildCategories(int parentCategoryId);

        public E.Category? GetFirstParentCategory(int childCategoryId);

        public Task<IEnumerable<E.Category>> GetParentCategoriesLine(int childCategoryId);
    }
}
