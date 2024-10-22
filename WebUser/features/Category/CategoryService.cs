using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Exceptions;
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

        public IEnumerable<E.Category> GetFirstGenChildCategories(int parentCategoryId) =>
            context.Categories.Where(q => q.ParentCategoryID == parentCategoryId);

        public async Task<IEnumerable<E.Category>> GetAllGenChildCategories(int parentCategoryId)
        {
            var childCategories = new List<E.Category>();
            var category =
                await context.Categories.Where(q => q.ID == parentCategoryId).FirstOrDefaultAsync()
                ?? throw new CategoryNotFoundException(parentCategoryId);
            GetChildCategoriesRec(category, childCategories);
            childCategories.RemoveAt(0); //remove the requested category object
            return childCategories;
        }

        private static void GetChildCategoriesRec(E.Category parentCategory, List<E.Category> childCategories)
        {
            childCategories.Add(parentCategory);
            if (parentCategory.Subcategories != null)
            {
                foreach (var subcategory in parentCategory.Subcategories)
                {
                    GetChildCategoriesRec(subcategory, childCategories);
                }
            }
        }

        public E.Category? GetFirstParentCategory(int childCategoryId) =>
            context.Categories.Include(q => q.ParentCategory).FirstOrDefault(q => q.ID == childCategoryId)?.ParentCategory;

        public async Task<IEnumerable<E.Category>> GetParentCategoriesLine(int childCategoryId)
        {
            var parentCategories = new List<E.Category>();
            var category =
                await context.Categories.Where(q => q.ID == childCategoryId).FirstOrDefaultAsync()
                ?? throw new CategoryNotFoundException(childCategoryId);
            GetParentCategoriesRec(category, parentCategories);
            parentCategories.RemoveAt(0); //remove the requested category object
            parentCategories.Reverse(); //reverse categories so that originator category will be the first one
            return parentCategories;
        }

        private static void GetParentCategoriesRec(E.Category childCategory, List<E.Category> parentCategories)
        {
            parentCategories.Add(childCategory);
            var parent = childCategory.ParentCategory;
            if (parent != null)
            {
                GetChildCategoriesRec(parent, parentCategories);
            }
        }
    }
}
