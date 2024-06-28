using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.Exceptions;
using WebUser.features.Product.Exceptions;

namespace WebUser.features.Cart.functions
{
    public class RemoveProductFromCart
    {
        public class RemoveProductFromCartCommand : IRequest
        {
            public int ProductId { get; set; }
            public int CartId { get; set; }
        }

        public class Handler : IRequestHandler<RemoveProductFromCartCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(RemoveProductFromCartCommand request, CancellationToken cancellationToken)
            {
                if (!await dbcontext.Carts.AnyAsync(q => q.ID == request.CartId, cancellationToken: cancellationToken))
                    throw new CartNotFoundException(request.CartId);
                if (!await dbcontext.Products.AnyAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken))
                    throw new ProductNotFoundException(request.ProductId);
                var cartItem = await dbcontext.CartItems.FirstOrDefaultAsync(
                    q => q.CartID == request.CartId && q.ProductID == request.ProductId,
                    cancellationToken: cancellationToken
                );
                if (cartItem != null)
                {
                    dbcontext.CartItems.Remove(cartItem);
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
