using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.data;

namespace Data.DTO.category
{
    public class UpdateCategoryDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public CategoryDTO Parent { get; set; }
        public int ParentId { get; set; }
        public List<CategoryDTO> Children { get; set; }

        public List<ProductDTO> Products { get; set; }
    }
}
