using E=WebUser.Domain.entities;

namespace WebUser.features.Category.DTO
{
    public class UpdateCategoryDTO
    {
        public string Name { get; set; }
        public ICollection<E.AttributeName> Attributes { get; set; }
        public E.Category ParentCategory { get; set; }
        public ICollection<E.Category> Subcategories { get; set; }
    }
}
