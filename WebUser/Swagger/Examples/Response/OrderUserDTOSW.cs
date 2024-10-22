namespace WebUser.features.Order.DTO;

using Swashbuckle.AspNetCore.Filters;
using WebUser.features.OrderProduct.DTO;
using WebUser.features.Product.DTO;

public class OrderUserDTOSW : IExamplesProvider<OrderUserDTO>
{
    public OrderUserDTO GetExamples() =>
        new()
        {
            ID = 1,
            DeliveryAddress = "street 6",
            DeliveryMethod = 1,
            PaymentMethod = 2,
            Status = true,
            Payment = 220,
            CreatedAt = DateTime.UtcNow,
            PointsUsed = 100,
            OrderProducts = new List<OrderProductDTO>
            {
                new()
                {
                    ID = 1,
                    Amount = 2,
                    Product = new ProductMinDTO { ID = 1, Name = "apple" },
                    FinalPrice = 20.0,
                    TotalFinalPrice = 40,
                },
                new()
                {
                    ID = 2,
                    Amount = 10,
                    Product = new ProductMinDTO { ID = 2, Name = "banana" },
                    FinalPrice = 30,
                    TotalFinalPrice = 300,
                },
            },
        };
}
