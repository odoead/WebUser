using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Order.DTO;
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
            public string UserId { get; set; } = string.Empty;
            public string Codes { get; set; } = "";
            public int Points { get; set; }
            public string DeliveryAddress { get; set; }
            public int DeliveryMethod { get; set; }
            public int PaymentMethod { get; set; }
            public int CartId { get; set; }
        }

        public class Handler : IRequestHandler<CreateOrderCommand, CreateOrderDTO>
        {
            private readonly DB_Context dbcontext;
            private readonly UserManager<E.User> userManager;
            private readonly IServiceWrapper service;

            public Handler(DB_Context context, UserManager<E.User> userManager, IServiceWrapper serviceWrapper)
            {
                dbcontext = context;
                this.userManager = userManager;
                this.service = serviceWrapper;
            }

            public async Task<CreateOrderDTO> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
            {
                using var transaction = await dbcontext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var user = await userManager.FindByEmailAsync(command.UserId);

                    var cartItems = await dbcontext.CartItems
                        .Include(ci => ci.Product)
                        .Where(ci => ci.CartID == command.CartId && ci.Cart.UserID == command.UserId)
                        .ToListAsync(cancellationToken);

                    foreach (var item in cartItems)
                    {
                        var product = await dbcontext.Products
                            .FromSqlRaw("SELECT * FROM Products WITH (UPDLOCK) WHERE ID = {0}", item.ProductID)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (product == null || product.Stock < item.Amount)
                        {
                            throw new InvalidOperationException($"Product {item.ProductID} does not have sufficient stock");
                        }

                        product.Stock -= item.Amount;
                        product.ReservedStock += item.Amount;
                        await dbcontext.SaveChangesAsync(cancellationToken);
                    }

                    var itemIdQuantities = cartItems.Select(ci => (productId: ci.ProductID, quantity: ci.Amount)).ToList();
                    var orderResult = await service.Pricing.GenerateOrderAsync(itemIdQuantities, user, command.Codes, command.Points);
                    if (orderResult.newPoint != null)
                    {
                        user.Points.Add(orderResult.newPoint);
                        await userManager.UpdateAsync(user);
                    }

                    var order = new E.Order
                    {
                        UserID = command.UserId,
                        DeliveryAddress = command.DeliveryAddress,
                        DeliveryMethod = command.DeliveryMethod,
                        PaymentMethod = command.PaymentMethod,
                        PointsUsed = Math.Min(orderResult.activatedPoints.Sum(q => q.Value), command.Points),
                        Payment = orderResult.order.OrderCost,
                        IsCompleted = false,
                        CreatedAt = DateTime.UtcNow,
                        Points = orderResult.activatedPoints,
                    };

                    foreach (var product in orderResult.order.Products)
                    {
                        var orderProduct = new E.OrderProduct
                        {
                            ProductID = product.ProductId,
                            Amount = product.Quantity,
                            ActivatedCoupons = orderResult.activatedCoupons.Where(c => c.ProductID == product.ProductId).ToList(),
                            FinalPrice = product.FinalSinglePrice,
                        };
                        order.OrderProducts.Add(orderProduct);
                    }

                    dbcontext.Orders.Add(order);
                    await dbcontext.SaveChangesAsync(cancellationToken);

                    // Convert reserved stock to consumed stock
                    foreach (var item in cartItems)
                    {
                        var product = await dbcontext.Products.FindAsync(item.ProductID);
                        product.ReservedStock -= item.Amount;
                        await dbcontext.SaveChangesAsync(cancellationToken);
                    }

                    await transaction.CommitAsync(cancellationToken);

                    #region mapping
                    var createOrderDTO = new CreateOrderDTO
                    {
                        ID = order.ID,
                        DeliveryAddress = order.DeliveryAddress,
                        DeliveryMethod = order.DeliveryMethod,
                        PaymentMethod = order.PaymentMethod,
                        Status = order.IsCompleted,
                        Payment = order.Payment,
                        CreatedAt = order.CreatedAt,
                        UserID = command.UserId,
                        PointsUsed = order.PointsUsed,
                        OrderProducts = order.OrderProducts.Select(op => new OrderProductDTO
                        {
                            ID = op.ID,
                            Amount = op.Amount,
                            FinalPrice = op.FinalPrice,
                            TotalFinalPrice = op.Amount * op.FinalPrice,
                            Product = new ProductMinDTO
                            {
                                Price = op.Product.Price,
                                ID = op.Product.ID,
                                Name = op.Product.Name
                            }
                        }).ToList()
                    };
                    #endregion
                    return createOrderDTO;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

    }
}
