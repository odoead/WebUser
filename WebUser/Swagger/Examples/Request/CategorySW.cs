using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class CategorySW : IExamplesProvider<Category>
    {
        public Category GetExamples() => throw new NotImplementedException();
    }
}
