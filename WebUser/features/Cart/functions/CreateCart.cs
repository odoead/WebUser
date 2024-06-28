using AutoMapper;
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
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<CartDTO> Handle(CreateCartCommand request, CancellationToken cancellationToken)
            {
                var user =
                    await dbcontext.Users.FirstOrDefaultAsync(q => q.Id == request.UserId, cancellationToken: cancellationToken)
                    ?? throw new UserNotFoundException(request.UserId);
                var cart = new E.Cart { User = user, Items = null };
                if (!await dbcontext.Carts.AnyAsync(q => q.User == user, cancellationToken: cancellationToken))
                {
                    await dbcontext.Carts.AddAsync(cart, cancellationToken);
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
                var results = mapper.Map<CartDTO>(cart);
                return results;
            }
        }
    }
}
