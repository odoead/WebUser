using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebUser.Domain.entities
{
    public class User : IdentityUser
    {
        /*public User()
        {
            return new User
            {
                AccessFailedCount = null,
                ConcurrencyStamp = null,
                DateCreated = null,
                Email = null,
                EmailConfirmed = null,
                FirstName = null,
                Id = null,
                LastName = null,
                LockoutEnabled = null,
                LockoutEnd = null,
                NormalizedEmail = null,
                NormalizedUserName = null,
                Orders = null,
                PasswordHash = null,
                PhoneNumber = null,
                PhoneNumberConfirmed = null,
                Points = null,
                TwoFactorEnabled = null,
                SecurityStamp = null,
                UserName = null,
            };
        }*/
        public string RefreshToken {  get; set; }
        public DateTime RefreshTokenExpireDate {  get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(60, ErrorMessage = "Surname can't be longer than 60 characters")]
        public string? LastName { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<Point> Points { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
