using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class OrderProduct
    {
        [Key]
        public int ID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed,excluding 0")]
        [Required]
        public int Amount { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double FinalPrice { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("OrderID")]
        public Order Order { get; set; }
        public int OrderID { get; set; }
        public ICollection<Coupon> ActivatedCoupons { get; set; } = new List<Coupon>();
    }
}
