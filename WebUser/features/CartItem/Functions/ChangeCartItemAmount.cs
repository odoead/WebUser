using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.CartItem.Exceptions;

namespace WebUser.features.CartItem.Functions
{
    public class ChangeCartItemAmount
    {
        //intput
        public class ChangeCartItemAmountCommand : IRequest
        {
            public int CartItemId { get; set; }
            public int NewAmount { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<ChangeCartItemAmountCommand>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task Handle(ChangeCartItemAmountCommand request, CancellationToken cancellationToken)
            {
                var cartItem =
                    await dbcontext.CartItems.FirstOrDefaultAsync(ci => ci.ID == request.CartItemId, cancellationToken)
                    ?? throw new CartItemNotFoundException(request.CartItemId);
                if (cartItem.Product.Stock <= request.NewAmount)
                {
                    cartItem.Amount = cartItem.Product.Stock;
                }
                else
                    cartItem.Amount = request.NewAmount;
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
