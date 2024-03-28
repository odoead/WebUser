using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E=WebUser.Domain.entities;
using WebUser.Domain.entities;

namespace WebUser.features.Point.DTO
{
    public class PointDTO
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public int BalanceLeft { get; set; }
        public bool isExpirable { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public E.User? User { get; set; }= null;
        public E.Order? Order { get; set; }=null;
    }
}
