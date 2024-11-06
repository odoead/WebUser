using WebUser.features.AttributeName.DTO;

namespace WebUser.features.Category.DTO
{
    public class CategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<AttributeNameMinDTO> Attributes { get; set; } = new List<AttributeNameMinDTO>();
        public CategoryMinDTO? ParentCategory { get; set; }
        public List<CategoryMinDTO> Subcategories { get; set; } = new List<CategoryMinDTO>();
    }
}
