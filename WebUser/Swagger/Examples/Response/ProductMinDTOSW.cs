namespace WebUser.features.Product.DTO;

using Swashbuckle.AspNetCore.Filters;

public class ProductMinDTOSW : IExamplesProvider<ProductMinDTO>
{
    public ProductMinDTO GetExamples() =>
        new ProductMinDTO
        {
            ID = 1,
            Name = "smartphone",
            Price = 1000
        };
}
