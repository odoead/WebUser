using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Data.DBData;

namespace Entities.data
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Image? Image { get; set; }
        public int ImageID { get; set; }


    }
}
