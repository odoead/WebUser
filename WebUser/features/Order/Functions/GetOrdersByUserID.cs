using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Order.DTO;
using WebUser.features.OrderProduct.DTO;
using WebUser.features.Point.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.User.Exceptions;

namespace WebUser.features.Order.Functions
{
    public class GetOrdersByUser
    {
        //input
        public class GetOrdersByUserQuery : IRequest<ICollection<OrderDTO>>
        {
            public string UserId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetOrdersByUserQuery, ICollection<OrderDTO>>
        {
            private readonly DB_Context dbcontext;


            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<ICollection<OrderDTO>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
            {
                var user =
                    await dbcontext.Users.FirstOrDefaultAsync(q => q.Id == request.UserId, cancellationToken: cancellationToken)
                    ?? throw new UserNotFoundException(request.UserId);
                var orders = await dbcontext
                    .Orders.Include(q => q.Points)
                    .Include(q => q.OrderProduct)
                    .ThenInclude(q => q.Product)
                    .Where(q => q.UserID == user.Id)
                    .ToListAsync(cancellationToken: cancellationToken);

                var orderDTOs = new List<OrderDTO>();
                foreach (var order in orders)
                {
                    var orderDTO = new OrderDTO
                    {
                        ID = order.ID,
                        CreatedAt = order.CreatedAt,
                        DeliveryAddress = order.DeliveryAddress,
                        DeliveryMethod = order.DeliveryMethod,
                        PaymentMethod = order.PaymentMethod,
                        Status = order.Status,
                        Payment = order.Payment,
                        UserID = order.UserID,
                        PointsUsed = order.PointsUsed,
                        OrderProducts = order
                            .OrderProduct.Select(op => new OrderProductDTO
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
                    orderDTOs.Add(orderDTO);
                }
                return orderDTOs;
            }
        }
    }
}
