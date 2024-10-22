namespace WebUser.Domain.entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Review
{
    [Key]
    public int ID { get; set; }

    [Required]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public string Header { get; set; }
    public string Body { get; set; }

    [Range(1, 5, ErrorMessage = "Provided values must be in 1-5 range")]
    public int Rating { get; set; }

    [ForeignKey("UserID")]
    public User User { get; set; }
    public string UserID { get; set; }

    [ForeignKey("ProductID")]
    public Product Product { get; set; }
    public int ProductID { get; set; }
}
