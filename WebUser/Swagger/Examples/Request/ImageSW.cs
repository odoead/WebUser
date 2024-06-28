using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class ImageSW : IExamplesProvider<Image>
    {
        public Image GetExamples() => throw new NotImplementedException();
    }
}
