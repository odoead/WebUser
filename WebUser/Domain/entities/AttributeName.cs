namespace WebUser.Domain.entities;

using System.ComponentModel.DataAnnotations;

public class AttributeName
{
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<AttributeNameCategory> Categories { get; set; } = new List<AttributeNameCategory>();
    public ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();

}
