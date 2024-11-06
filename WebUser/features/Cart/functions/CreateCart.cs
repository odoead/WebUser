using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.DTO;
using WebUser.features.User.Exceptions;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart.functions
{
    public class CreateCart
    {
        //input
        public class CreateCartCommand : IRequest<CartDTO>
        {
            public string UserId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateCartCommand, CartDTO>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<CartDTO> Handle(CreateCartCommand request, CancellationToken cancellationToken)
            {
                var user =
                    await dbcontext.Users.Include(q => q.Cart).ThenInclude(q => q.Items).FirstOrDefaultAsync(q => q.Id == request.UserId, cancellationToken: cancellationToken)
                    ?? throw new UserNotFoundException(request.UserId);

                var existingCart = user.Cart;

                if (existingCart != null)
                {
                    return new CartDTO
                    {
                        ID = existingCart.ID,
                        Items = existingCart.Items.Select(item => new CartItemDTO { ID = item.ID, Amount = item.Amount }).ToList(),
                        UserId = user.Id,
                    };
                }

                var cart = new E.Cart { User = user, Items = new List<E.CartItem>() };

                await dbcontext.Carts.AddAsync(cart, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);

                var results = new CartDTO
                {
                    ID = cart.ID,
                    Items = new List<CartItemDTO>(),
                    UserId = user.Id,
                };

                return results;
            }
        }
    }
}
