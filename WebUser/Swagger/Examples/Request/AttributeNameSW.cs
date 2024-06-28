using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class AttributeNameSW : IExamplesProvider<AttributeName>
    {
        public AttributeName GetExamples() => throw new NotImplementedException();
    }
}
