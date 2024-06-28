namespace WebUser.Domain.entities;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Keyless]
public class AttributeNameCategory
{
    [ForeignKey("AttributeNameID")]
    public AttributeName AttributeName { get; set; }
    public int AttributeNameID { get; set; }

    [ForeignKey("CategoryID")]
    public Category Category { get; set; }
    public int CategoryID { get; set; }
}
