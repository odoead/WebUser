using WebUser.features.AttributeName.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Category.DTO
{
    public class UpdateCategoryDTO
    {
        public string Name { get; set; }
        public List<AttributeNameDTO> Attributes { get; set; }
        public E.Category ParentCategory { get; set; }
        public List<E.Category> Subcategories { get; set; }
    }
}
