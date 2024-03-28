using Entities.data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DBData
{
    public class BrandDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<ProductDTO> Products { get; set; }
    }
}
