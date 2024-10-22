using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Product.Exceptions;
using E = WebUser.Domain.entities;

namespace WebUser.features.Product.Functions
{
    public class ChangeAmountDeleteProduct
    {
        //input
        public class DeleteProductCommand : IRequest
        {
            public int ID { get; set; }
            public int NewAmount { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<DeleteProductCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var product =
                    await dbcontext.Products.Where(q => q.ID == request.ID).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new ProductNotFoundException(request.ID);
                var prevStock = product.Stock;

                if (request.NewAmount < 0 || request.NewAmount - product.ReservedStock < 0)
                {
                    dbcontext.Products.Remove(product);
                }
                else
                {
                    var isPrevStockWasEmptyOrFullReserved = prevStock - product.ReservedStock == 0 || prevStock == 0;
                    var isNewStockHaveFreeProds = request.NewAmount - product.ReservedStock > 0; // Reserved stock cannot be changed
                    if (isPrevStockWasEmptyOrFullReserved && isNewStockHaveFreeProds)
                    {
                        await NotifyUsersAboutProduct(product, cancellationToken);
                    }
                    product.Stock = request.NewAmount;
                }

                await dbcontext.SaveChangesAsync(cancellationToken);
            }

            private async Task NotifyUsersAboutProduct(E.Product product, CancellationToken cancellationToken)
            {
                var notifications = await dbcontext
                    .RequestNotifications.Include(q => q.User)
                    .Include(q => q.Product)
                    .Where(q => q.ProductID == product.ID)
                    .ToListAsync(cancellationToken);

                foreach (var notification in notifications)
                {
                    await ProductEmailNotification.Notify(notification.User, notification.Product);
                    dbcontext.RequestNotifications.Remove(notification);
                }
            }
        }
    }
}
