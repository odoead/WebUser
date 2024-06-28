using Swashbuckle.AspNetCore.Filters;

namespace WebUser.features.Image.DTO
{
    public class ImageDTOSW : IExamplesProvider<ImageDTO>
    {
        public ImageDTO GetExamples() => new ImageDTO { ID = 22, ImageContent = new byte[] { } };
    }
}
