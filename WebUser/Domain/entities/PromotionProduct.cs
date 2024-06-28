namespace WebUser.Domain.entities;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Keyless]
public class PromotionProduct
{
    [ForeignKey("PromotionID")]
    public Promotion Promotion { get; set; }
    public int PromotionID { get; set; }

    [ForeignKey("ProductID")]
    public Product Product { get; set; }
    public int ProductID { get; set; }
}
