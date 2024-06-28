using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.DTO;
using WebUser.features.Cart.Exceptions;

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
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<CartDTO> Handle(GetCartByUserIDQuery request, CancellationToken cancellationToken)
            {
                var cart =
                    await dbcontext.Carts.Where(q => q.User.Id == request.UserId).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new CartNotFoundException(-1);
                //throw new UserNotFoundException(request.UserId);
                var results = mapper.Map<CartDTO>(cart);
                return results;
            }
        }
    }
}
