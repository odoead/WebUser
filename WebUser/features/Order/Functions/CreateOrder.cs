using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.Exceptions;
using WebUser.features.Order.DTO;
using WebUser.features.OrderProduct.DTO;
using WebUser.features.Product.DTO;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Order.Functions
{
    public class CreateOrder
    {
        //input
        public class CreateOrderCommand : IRequest<CreateOrderDTO>
        {
            public ClaimsPrincipal UserClaims { get; set; }
            public string Codes { get; set; } = "";
            public int Points { get; set; }
            public string DeliveryAddress { get; set; }
            public int DeliveryMethod { get; set; }
            public int PaymentMethod { get; set; }
            public int CartId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateOrderCommand, CreateOrderDTO>
        {
            private readonly DB_Context dbcontext;
            private readonly IServiceWrapper service;
            private readonly UserManager<E.User> userManager;

            public Handler(DB_Context context, IServiceWrapper wrapper, UserManager<E.User> userManager)
            {
                dbcontext = context;

                service = wrapper;
                this.userManager = userManager;
            }

            public async Task<CreateOrderDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                using (var transaction = await dbcontext.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        var userId = request.UserClaims.FindFirstValue(ClaimTypes.NameIdentifier);
                        var user = await userManager.FindByIdAsync(userId);

                        //discount->coupon->promo->points
                        double totalCost = 0;
                        if (!await dbcontext.Carts.AnyAsync(q => q.ID == request.CartId))
                        {
                            throw new CartNotFoundException(request.CartId);
                        }
                        var cart = dbcontext
                            .Carts.Include(q => q.User)
                            .Include(q => q.Items)
                            .ThenInclude(q => q.Product)
                            .Where(q => q.ID == request.CartId);
                        List<(E.CartItem, double)> itemDiscounts = new List<(E.CartItem, double)>();
                        itemDiscounts.AddRange(SetStartNullDiscounts(cart));
                        E.Point addPoint;
                        List<E.Point> activatedPoints = new List<E.Point>();
                        var promoDisc = new List<(E.CartItem, double)>();
                        //disc
                        itemDiscounts.AddRange(service.Order.ApplyDiscount(cart));
                        //coupon
                        var activatedCoupons = new List<(E.CartItem, E.Coupon)>();
                        var couponItemDiscounts = new List<(E.CartItem, double)>();
                        (couponItemDiscounts, activatedCoupons) = service.Order.ApplyCoupons(cart, request.Codes);
                        itemDiscounts.AddRange(couponItemDiscounts);
                        //promo
                        (promoDisc, addPoint) = service.Order.ApplyPromos(cart);
                        itemDiscounts.AddRange(promoDisc);
                        //point

                        activatedPoints.AddRange(service.Order.ApplyPoints(user, request.Points));

                        var orderProducts = service.OrderProduct.CreateOrderProdsFromCartItemsDiscounts(itemDiscounts, activatedCoupons);
                        foreach (var item in orderProducts)
                        {
                            if (item.Product.Stock - item.Product.ReservedStock < item.Amount)
                            {
                                throw new Exception($"not enought free stock items {item.ProductID}");
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
                            User = user,
                            OrderProduct = orderProducts,
                            Points = activatedPoints,
                            PointsUsed = request.Points,
                            Payment = totalCost,
                        };
                        user.Points.Add(addPoint);
                        await dbcontext.Orders.AddAsync(order, cancellationToken);
                        await dbcontext.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        #region mapping
                        //var results = mapper.Map<OrderDTO>(order);
                        var results = new CreateOrderDTO
                        {
                            ID = order.ID,
                            CreatedAt = order.CreatedAt,
                            DeliveryAddress = order.DeliveryAddress,
                            DeliveryMethod = order.DeliveryMethod,
                            PaymentMethod = order.PaymentMethod,
                            Status = order.Status,
                            Payment = order.Payment,
                            PointsUsed = order.PointsUsed,
                            UserID = order.UserID,
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
                        };
                        #endregion
                        return results;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        throw ex;
                    }
                }
            }

            private static List<(E.CartItem, double)> SetStartNullDiscounts(IQueryable<E.Cart> currentCart)
            {
                List<(E.CartItem, double)> seedValues = new();
                var cartItems = currentCart.SelectMany(c => c.Items);
                foreach (var item in cartItems)
                {
                    seedValues.Add((item, 0));
                }
                return seedValues;
            }
        }
    }
}
