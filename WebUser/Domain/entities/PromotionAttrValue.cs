namespace WebUser.Domain.entities;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Keyless]
public class PromotionAttrValue
{
    [ForeignKey("PromotionID")]
    public Promotion Promotion { get; set; }
    public int PromotionID { get; set; }

    [ForeignKey("AttributeValueID")]
    public AttributeValue AttributeValue { get; set; }
    public int AttributeValueID { get; set; }
}
