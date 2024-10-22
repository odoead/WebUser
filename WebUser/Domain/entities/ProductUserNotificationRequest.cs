namespace WebUser.Domain.entities;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Keyless]

public class ProductUserNotificationRequest
{
    [ForeignKey("ProductID")]
    public Product Product { get; set; }
    public int ProductID { get; set; }

    [ForeignKey("UserID")]
    public User User { get; set; }
    public string UserID { get; set; }
}
