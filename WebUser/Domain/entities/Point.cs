using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class Point
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number and 0 allowed")]
        public int Value { get; set; }
        public int BalanceLeft {  get; set; }
        [Required]
        public bool isExpirable { get; set; }
        [Required]
        public bool IsUsed { get; set;}
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        public int? OrderId { get; set; }
    }
}
