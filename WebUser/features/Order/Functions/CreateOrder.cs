using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.Exceptions;
using WebUser.features.Order.DTO;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Order.Functions
{
    public class CreateOrder
    {
        //input
        public class CreateOrderCommand : IRequest<OrderDTO>
        {
            public E.User User { get; set; }
            public string Codes { get; set; }
            public int Points { get; set; }
            public string DeliveryAddress { get; set; }
            public int DeliveryMethod { get; set; }
            public int PaymentMethod { get; set; }
            public int CartId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateOrderCommand, OrderDTO>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;
            private readonly IServiceWrapper service;

            public Handler(DB_Context context, IMapper mapper, IServiceWrapper wrapper)
            {
                dbcontext = context;
                this.mapper = mapper;
                service = wrapper;
            }

            public async Task<OrderDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                //discount->coupon->promo->points
                double totalCost = 0;
                List<(E.CartItem, double)> itemDiscounts = new List<(E.CartItem, double)>();
                E.Point addPoint;
                List<E.Point> activatedPoints = new List<E.Point>();
                var cart =
                    await dbcontext.Carts.FirstOrDefaultAsync(q => q.ID == request.CartId, cancellationToken: cancellationToken)
                    ?? throw new CartNotFoundException(request.CartId);
                //disc
                itemDiscounts.AddRange(service.Order.ApplyDiscount(cart));
                //coupon
                itemDiscounts.AddRange(service.Order.ApplyCoupons(cart, request.Codes));
                //promo
                var promoDisc = new List<(E.CartItem, double)>();
                (promoDisc, addPoint) = service.Order.ApplyPromos(cart);
                itemDiscounts.AddRange(promoDisc);
                //point
                activatedPoints.AddRange(service.Order.ApplyPoints(request.User, request.Points));
                var orderProducts = service.OrderProduct.CreateOrderProdsFromCartItemsDiscounts(itemDiscounts);
                foreach (var item in orderProducts)
                {
                    if (item.Product.Stock <= item.Amount)
                    {
                        throw new Exception($"not enought stock items ");
                    }
                    item.Product.ReservedStock += item.Amount;
                    totalCost += item.FinalPrice * item.Amount;
                }
                if (activatedPoints.Any())
                {
                    totalCost -= request.Points;
                }
                var order = new E.Order
                {
                    CreatedAt = DateTime.UtcNow,
                    DeliveryAddress = request.DeliveryAddress,
                    DeliveryMethod = request.DeliveryMethod,
                    PaymentMethod = request.PaymentMethod,
                    Status = false,
                    User = request.User,
                    OrderProduct = orderProducts,
                    Points = activatedPoints,
                    PointsUsed = request.Points,
                    Payment = totalCost,
                };
                request.User.Points.Add(addPoint);
                await dbcontext.Orders.AddAsync(order, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);
                var results = mapper.Map<OrderDTO>(order);
                return results;
            }
        }
    }
}
