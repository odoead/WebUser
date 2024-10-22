using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebUser.Domain.entities
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(60, ErrorMessage = "Surname can't be longer than 60 characters")]
        public string? LastName { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<Point> Points { get; set; } = new List<Point>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductUserNotificationRequest> RequestNotifications { get; set; }

    }
}
