namespace WebUser.features.Category.DTO;

using WebUser.features.AttributeValue.DTO;

public class CategoryAttributeFiltersDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public List<CategoryMinDTO> ParentRouteCategories { get; set; } = new List<CategoryMinDTO>();
    public List<CategoryMinDTO> Subcategories { get; set; } = new List<CategoryMinDTO>();
    public List<AttributeNameValueDTO> Attributes { get; set; } = new List<AttributeNameValueDTO>();
    public bool IncludesChildCategories { get; set; }
}

public class SearchAttributeFiltersDTO
{
    public List<CategoryMinDTO> СategoriesOfFoundItems { get; set; } = new List<CategoryMinDTO>();
    public List<AttributeNameValueDTO> Attributes { get; set; } = new List<AttributeNameValueDTO>();
}
