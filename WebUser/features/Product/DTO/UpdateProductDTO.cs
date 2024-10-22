namespace WebUser.features.Product.DTO;
public class UpdateProductDTO
{
    public string Description { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
    public ICollection<int> AttributeValueIds { get; set; }
}
