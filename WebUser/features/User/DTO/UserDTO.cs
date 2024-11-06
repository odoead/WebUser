using WebUser.features.Order.DTO;
using WebUser.features.Point.DTO;

namespace WebUser.features.User.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public List<OrderUserDTO> Orders { get; set; } = new List<OrderUserDTO>();
        public List<PointMinDTO> Points { get; set; } = new List<PointMinDTO>();
    }
}
