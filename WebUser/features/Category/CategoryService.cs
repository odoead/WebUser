using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Interfaces;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class CategoryService : ICategoryService
    {
        private readonly DB_Context context;

        public CategoryService(DB_Context context)
        {
            this.context = context;
        }

        public IEnumerable<E.Category> ShowFirstChildCategories(int categoryId) => context.Categories.Where(q => q.ParentCategory.ID == categoryId);

        public async Task<IEnumerable<E.Category>> ShowAllChildCategories(int categoryId)
        {
            var childCategories = new List<E.Category>();
            var category = await context.Categories.Where(q => q.ID == categoryId).FirstOrDefaultAsync();
            GetChildCategoriesRec(category, childCategories);
            return childCategories;
        }

        private void GetChildCategoriesRec(E.Category category, List<E.Category> childCategories)
        {
            childCategories.Add(category);
            if (category.Subcategories != null)
            {
                foreach (var subcategory in category.Subcategories)
                {
                    GetChildCategoriesRec(subcategory, childCategories);
                }
            }
        }
    }
}
