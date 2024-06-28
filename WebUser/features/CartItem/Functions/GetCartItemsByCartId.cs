using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.CartItem.DTO;

namespace WebUser.features.CartItem.Functions
{
    public class GetCartItemsByCartId
    { //input
        public class GetByCartIDQuery : IRequest<ICollection<CartItemDTO>>
        {
            public int CartId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetByCartIDQuery, ICollection<CartItemDTO>>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<CartItemDTO>> Handle(GetByCartIDQuery request, CancellationToken cancellationToken)
            {
                var items = await dbcontext.CartItems.Where(q => q.CartID == request.CartId).ToListAsync(cancellationToken: cancellationToken);
                return mapper.Map<ICollection<CartItemDTO>>(items);
            }
        }
    }
}
