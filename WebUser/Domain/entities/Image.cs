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
        [ForeignKey("UserID")]
        public User? User { get; set; }
        public int? UserID { get; set; }
        [ForeignKey("ProductID")]
        public Product? Product { get; set; }
        public int? ProductID { get; set; }
    }
}
