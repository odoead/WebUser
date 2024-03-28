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
    public class OrderDTO
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }

        public UserDTO User { get; set; }
        public int UserID {  get; set; }

        public List<ProductDTO> Products { get; set; }
    }
}
