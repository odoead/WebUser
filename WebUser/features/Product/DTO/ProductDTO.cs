using System.ComponentModel.DataAnnotations;
using E=WebUser.Domain.entities;

namespace WebUser.features.Product.DTO
{
    public class ProductDTO
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public ICollection<E.Image>? Images { get; set; } = null;
        public ICollection<E.AttributeValue>? AttributeValues { get; set; } = null;
        public ICollection<E.OrderProduct>? OrderProduct { get; set; } = null;
    }
}
