using AutoMapper;
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
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<CartDTO> Handle(GetByIDCartQuery request, CancellationToken cancellationToken)
            {
                var cart =
                    await dbcontext.Carts.Where(q => q.ID == request.Id).FirstOrDefaultAsync(cancellationToken)
                    ?? throw new CartNotFoundException(request.Id);
                var results = mapper.Map<CartDTO>(cart);
                return results;
            }
        }
    }
}
