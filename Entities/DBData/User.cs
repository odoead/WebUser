using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Data.DBData;

namespace Entities.data
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(60, ErrorMessage = "Surname can't be longer than 60 characters")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters")]
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public DateTime RegistrationDate { get; set; }

        [ForeignKey("ImageID")]
        public Image? Image { get; set; }
        public int ImageID { get; set; }
    }
}
