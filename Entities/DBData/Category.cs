using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.data;

namespace Data.DBData
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        [ForeignKey("ParentId")]
        public Category Parent { get; set; }
        public int ParentId { get; set; }
        public ICollection<Category> Children { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
