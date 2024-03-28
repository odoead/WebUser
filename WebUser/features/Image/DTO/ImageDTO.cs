using E = WebUser.Domain.entities;

namespace WebUser.features.Image.DTO
{
    public class ImageDTO
    {
        public int ID { get; set; }
        public byte[] ImageContent { get; set; }
        public E.User? User { get; set; } = null;
        public E.Product? Product { get; set; } = null;

    }
}
