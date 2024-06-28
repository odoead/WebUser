using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class PointSW : IExamplesProvider<Point>
    {
        public Point GetExamples() => throw new NotImplementedException();
    }
}
