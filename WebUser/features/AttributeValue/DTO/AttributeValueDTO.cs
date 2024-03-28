using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E = WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
namespace WebUser.features.AttributeValue.DTO
{
    public class AttributeValueDTO
    {
        public int ID { get; set; }

        public string Value { get; set; }
        public AttributeNameDTO AttributeName { get; set; } = null;
    }
}
