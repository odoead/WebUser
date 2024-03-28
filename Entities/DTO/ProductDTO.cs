using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Data.DBData;

namespace Entities.data
{

    public class ProductDTO
    {
        public int ID { get; set; }
        public string ProductName { get; set; }

        public double Price { get; set; }
        public string? Description { get; set; }
        public List<Image>? Images { get; set; }
        public BrandDTO Brand { get; set; }
        public int BrandID { get; set; }
    }
}
