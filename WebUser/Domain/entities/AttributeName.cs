namespace WebUser.Domain.entities;

using System.ComponentModel.DataAnnotations;

public class AttributeName
{
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<AttributeNameCategory> Categories { get; set; } = new List<AttributeNameCategory>();
    public List<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();

}
