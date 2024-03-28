using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Interfaces;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart
{
    public class CategoryRepo : ICategoryService
    {
        private DB_Context _Context;
        public CategoryRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.Category category)
        {
            _Context.categories.Add(category);
        }

        public void Delete(E.Category category)
        {
            _Context.categories.Remove(category);
        }

        public async Task<ICollection<E.Category>>? GetAllAsync()
        {
            return await _Context.categories.ToListAsync();
        }

        public async Task<E.Category>? GetByIdAsync(ObjectID<E.Category> Id)
        {

            return await _Context.categories.Where(q => q.ID == Id.Value).FirstOrDefaultAsync();

        }

        public async Task<bool> IsExistsAsync(ObjectID<E.Category> Id)
        {
            return await _Context.categories.AnyAsync(q => q.ID == Id.Value);
        }
        public void Update(E.Category category)
        {
            _Context.categories.Update(category);
        }
        
        public IEnumerable<E.Category> ShowFirstChildCategories(E.Category category)
        {
            return _Context.categories.Where(q => q.ParentCategory == category);
        }
        public IEnumerable<E.Category> ShowAllChildCategories(E.Category category)
        {
            var childCategories = new List<E.Category>();
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
