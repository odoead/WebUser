namespace WebUser.features.Category.DTO
{
    public class UpdateCategoryDTO
    {
        public string Name { get; set; }
        public List<int> AttributeNameIds { get; set; }
        public int ParentCategoryId { get; set; }
        public List<int> SubcategoryIds { get; set; }
    }
}
