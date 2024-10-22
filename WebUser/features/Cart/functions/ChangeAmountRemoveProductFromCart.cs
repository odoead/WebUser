using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.Exceptions;
using WebUser.features.CartItem.Exceptions;
using WebUser.features.Product.Exceptions;

namespace WebUser.features.Cart.functions
{
    public class ChangeAmountRemoveProductFromCart
    {
        public class ChangeAmountRemoveProductFromCartCommand : IRequest
        {
            public int ProductId { get; set; }
            public int CartId { get; set; }
            public int NewAmount { get; set; }
        }

        public class Handler : IRequestHandler<ChangeAmountRemoveProductFromCartCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(ChangeAmountRemoveProductFromCartCommand request, CancellationToken cancellationToken)
            {
                if (!await dbcontext.Carts.AnyAsync(q => q.ID == request.CartId, cancellationToken: cancellationToken))
                {
                    throw new CartNotFoundException(request.CartId);
                }
                if (!await dbcontext.Products.AnyAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken))
                {
                    throw new ProductNotFoundException(request.ProductId);
                }

                var cartItem =
                    await dbcontext
                        .CartItems.Include(q => q.Product)
                        .FirstOrDefaultAsync(
                            q => q.CartID == request.CartId && q.ProductID == request.ProductId,
                            cancellationToken: cancellationToken
                        ) ?? throw new CartItemNotFoundException(-1);

                if (request.NewAmount < 0)
                {
                    dbcontext.CartItems.Remove(cartItem);
                }
                else if (request.NewAmount - cartItem.Amount > cartItem.Product.Stock - cartItem.Product.ReservedStock)
                {
                    cartItem.Amount = cartItem.Product.Stock - cartItem.Product.ReservedStock;
                }
                else
                {
                    cartItem.Amount = request.NewAmount;
                }
                await dbcontext.SaveChangesAsync(cancellationToken);
                //
            }
        }
    }
}
