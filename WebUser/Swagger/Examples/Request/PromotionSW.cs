using Swashbuckle.AspNetCore.Filters;

namespace WebUser.Domain.entities
{
    public class PromotionSW : IExamplesProvider<Promotion>
    {
        public Promotion GetExamples() => throw new NotImplementedException();
    }
}
