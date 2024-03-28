using Entities.data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DBData
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage ="Creation date is required")]
        public DateTime DateCreated { get; set; }

        [ForeignKey("UserID")]
        public UserDTO User { get; set; }
        public int UserID {  get; set; }

        public ICollection<ProductDTO> Products { get; set; }
    }
}
