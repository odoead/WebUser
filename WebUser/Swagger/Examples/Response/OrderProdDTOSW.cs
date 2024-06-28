using Swashbuckle.AspNetCore.Filters;
using WebUser.features.Product.DTO;

namespace WebUser.features.OrderProduct.DTO
{
    public class OrderProductDTOSW : IExamplesProvider<OrderProductDTO>
    {
        public OrderProductDTO GetExamples() =>
            new OrderProductDTO
            {
                ID = 1,
                Amount = 2,
                Product = new ProductMinDTO { ID = 1, Name = "apple" },
                FinalPrice = 20.0,
                TotalFinalPrice = 40
            };
    }
}
