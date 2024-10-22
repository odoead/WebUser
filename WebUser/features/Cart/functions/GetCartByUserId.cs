using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.DTO;
using WebUser.features.Cart.Exceptions;
using WebUser.features.CartItem.DTO;
using WebUser.features.User.Exceptions;

namespace WebUser.features.Cart.functions
{
    public class GetCartByUserId
    {
        //input
        public class GetCartByUserIDQuery : IRequest<CartDTO>
        {
            public string UserId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetCartByUserIDQuery, CartDTO>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<CartDTO> Handle(GetCartByUserIDQuery request, CancellationToken cancellationToken)
            {
                var user =
                    await dbcontext.Users.FirstOrDefaultAsync(q => q.Id == request.UserId, cancellationToken: cancellationToken)
                    ?? throw new UserNotFoundException(request.UserId);
                var cart =
                    await dbcontext
                        .Carts.Include(q => q.Items)
                        .ThenInclude(q => q.Product)
                        .Where(q => q.User.Id == request.UserId)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new CartNotFoundException(-1);

                var results = new CartDTO
                {
                    ID = cart.ID,
                    UserId = cart.UserID,
                    Items = new List<CartItemDTO>(),
                };
                foreach (var item in cart.Items)
                {
                    results.Items.Add(
                        new CartItemDTO
                        {
                            Amount = item.Amount,
                            ID = item.ID,
                            ProductBasePrice = item.Product.Price,
                            ProductID = item.ProductID,
                            ProductName = item.Product.Name,
                        }
                    );
                }

                return results;
            }
        }
    }
}
