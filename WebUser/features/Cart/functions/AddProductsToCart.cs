using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.Exceptions;
using WebUser.features.CartItem.DTO;
using WebUser.features.Product.Exceptions;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart.functions
{
    public class AddProductsToCart
    {
        public class AddProductToCartCommand : IRequest
        {
            public int ProductId { get; set; }
            public int CartId { get; set; }
            public int Amount { get; set; }
        }

        public class Handler : IRequestHandler<AddProductToCartCommand>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
            }

            public async Task Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
            {
                var cart =
                    await dbcontext.Carts.Where(q => q.ID == request.CartId).FirstOrDefaultAsync(cancellationToken)
                    ?? throw new CartNotFoundException(request.CartId);
                var product =
                    await dbcontext.Products.Where(q => q.ID == request.CartId).FirstOrDefaultAsync(cancellationToken)
                    ?? throw new ProductNotFoundException(request.ProductId);
                var cartItem = cart.Items.FirstOrDefault(q => q.Product.ID == request.ProductId);
                if (cartItem == null)
                {
                    var newCartItem = new E.CartItem
                    {
                        Amount = request.Amount,
                        Cart = cart,
                        Product = product
                    };
                    cart.Items.Add(cartItem);
                }
                else
                {
                    cartItem.Amount = request.Amount;
                }
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
