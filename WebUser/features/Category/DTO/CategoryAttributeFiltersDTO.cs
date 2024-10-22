namespace WebUser.features.Category.DTO;

using WebUser.features.AttributeValue.DTO;

public class CategoryAttributeFiltersDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public ICollection<CategoryMinDTO>? ParentRouteCategories { get; set; }
    public ICollection<CategoryMinDTO>? Subcategories { get; set; }
    public ICollection<AttributeNameValueDTO>? Attributes { get; set; } = new List<AttributeNameValueDTO>();
    public bool IncludesChildCategories { get; set; }
}

public class SearchAttributeFiltersDTO
{
    public ICollection<CategoryMinDTO>? СategoriesOfFoundItems { get; set; } = new List<CategoryMinDTO>();
    public ICollection<AttributeNameValueDTO>? Attributes { get; set; } = new List<AttributeNameValueDTO>();
}
