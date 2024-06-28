using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DBData;

namespace Entities.data
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Price { get; set; }
        public string? Description { get; set; }
        public ICollection<Image>? Images { get; set; }

        [ForeignKey("BrandID")]
        public BrandDTO Brand { get; set; }
        public int BrandID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Only positive number and 0 allowed")]
        public int quantity { get; set; }
    }
}
