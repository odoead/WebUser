using WebUser.Data;
using WebUser.features.Image.Interfaces;

namespace WebUser.features.Image
{
    public class ImageService : IImageService
    {
        private readonly DB_Context _Context;

        public ImageService(DB_Context context)
        {
            _Context = context;
        }
    }
}
