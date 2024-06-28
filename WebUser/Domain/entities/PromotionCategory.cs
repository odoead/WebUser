namespace WebUser.Domain.entities;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Keyless]
public class PromotionCategory
{
    [ForeignKey("PromotionID")]
    public Promotion Promotion { get; set; }
    public int PromotionID { get; set; }

    [ForeignKey("CategoryID")]
    public Category Category { get; set; }
    public int CategoryID { get; set; }
}
