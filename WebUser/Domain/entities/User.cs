using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUser.Domain.entities
{
    public class User : IdentityUser
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(60, ErrorMessage = "Surname can't be longer than 60 characters")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        public ICollection<Point> Points { get; set; }
        public ICollection<Coupon> ActivatedCoupons { get; set; }

    }
}
