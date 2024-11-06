using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Order.DTO;
using WebUser.features.Point.DTO;
using WebUser.features.Product.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Order.Functions
{
    public class GetAllOrders
    {
        //input
        public class GetAllOrdersAsyncQuery : IRequest<PagedList<OrderDTO>>
        {
            public OrderRequestParameters Parameters { get; set; }

            public GetAllOrdersAsyncQuery(OrderRequestParameters parameters)
            {
                Parameters = parameters;
            }
        }

        //handler
        public class Handler : IRequestHandler<GetAllOrdersAsyncQuery, PagedList<OrderDTO>>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }
            public async Task<PagedList<OrderDTO>> Handle(GetAllOrdersAsyncQuery request, CancellationToken cancellationToken)
            {
                var data = dbcontext.Orders.Include(q => q.Points).Include(q => q.OrderProducts).ThenInclude(q => q.Product).AsQueryable();
                var orderDTOs = new List<OrderDTO>();
                foreach (var order in data)
                {
                    var orderDTO = new OrderDTO
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
                    orderDTOs.Add(orderDTO);
                }

                var pagedList = PagedList<OrderDTO>.PaginateList(
                    source: orderDTOs,
                    totalCount: await data.CountAsync(cancellationToken),
                    pageNumber: request.Parameters.PageNumber,
                    pageSize: request.Parameters.PageSize
                );

                return pagedList;
            }
        }
    }
}
