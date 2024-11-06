using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.DTO;
using WebUser.features.Cart.Exceptions;

namespace WebUser.features.Cart.functions
{
    public class GetCartByID
    {
        //input
        public class GetByIDCartQuery : IRequest<CartDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetByIDCartQuery, CartDTO>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<CartDTO> Handle(GetByIDCartQuery request, CancellationToken cancellationToken)
            {
                var cart =
                    await dbcontext
                        .Carts.Include(q => q.Items)
                        .ThenInclude(q => q.Product)
                        .Where(q => q.ID == request.Id)
                        .FirstOrDefaultAsync(cancellationToken) ?? throw new CartNotFoundException(request.Id);
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
