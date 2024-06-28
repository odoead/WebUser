using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.data;

namespace Data.DTO.category
{
    public class CategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public CategoryDTO Parent { get; set; }
        public int ParentId { get; set; }
        public List<CategoryDTO> Children { get; set; }

        public List<ProductDTO> Products { get; set; }
    }
}
