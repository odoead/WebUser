using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Exceptions;
using WebUser.features.Order.DTO;
using WebUser.features.Point.DTO;
using WebUser.features.Product.DTO;

namespace WebUser.features.Order.Functions
{
    public class GetOrderByID
    {
        //input
        public class GetByIDQuery : IRequest<OrderDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetByIDQuery, OrderDTO>
        {

            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<OrderDTO> Handle(GetByIDQuery request, CancellationToken cancellationToken)
            {
                var order =
                    await dbcontext
                        .Orders.Include(q => q.Points)
                        .Include(q => q.OrderProducts)
                        .ThenInclude(q => q.Product)
                        .FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken) ?? throw new CategoryNotFoundException(request.Id);

                var results = new OrderDTO
                {
                    ID = order.ID,
                    CreatedAt = order.CreatedAt,
                    DeliveryAddress = order.DeliveryAddress,
                    DeliveryMethod = order.DeliveryMethod,
                    PaymentMethod = order.PaymentMethod,
                    Status = order.IsCompleted,
                    Payment = order.Payment,
                    UserID = order.UserID,
                    PointsUsed = order.PointsUsed,
                    OrderProducts = order
                        .OrderProducts.Select(op => new OrderProductDTO
                        {
                            ID = op.ProductID,
                            Amount = op.Amount,
                            FinalPrice = op.FinalPrice,
                            TotalFinalPrice = op.FinalPrice * op.Amount,
                            Product = new ProductMinDTO
                            {
                                ID = op.ProductID,
                                Name = op.Product.Name,
                                Price = op.Product.Price,
                            },
                        })
                        .ToList(),
                    ActivatedPoints = order
                        .Points.Select(p => new PointMinDTO
                        {
                            BalanceLeft = p.BalanceLeft,
                            ID = p.ID,
                            IsUsed = p.IsUsed,
                            Value = p.Value,
                            IsActive = p.IsUsed,
                        })
                        .ToList(),
                };
                return results;
            }
        }
    }
}
