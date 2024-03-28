using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E=WebUser.Domain.entities;

namespace WebUser.features.CartItem.DTO
{
    public class CartItemDTO
    {
        public int ID { get; set; }
        public E.Cart Cart { get; set; }
        public int Amount { get; set; }
    }
}
