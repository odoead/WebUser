using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Image
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength]
        public byte[] ImageContent { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }
        public string? UserID { get; set; }

        [ForeignKey("ProductID")]
        public Product? Product { get; set; }
        public int? ProductID { get; set; }
    }
}
