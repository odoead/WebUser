namespace WebUser.Domain.entities;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Keyless]
public class ProductAttributeValue
{
    [ForeignKey("ProductID")]
    public Product Product { get; set; }
    public int ProductID { get; set; }

    [ForeignKey("AttributeValueID")]
    public AttributeValue AttributeValue { get; set; }
    public int AttributeValueID { get; set; }
}
