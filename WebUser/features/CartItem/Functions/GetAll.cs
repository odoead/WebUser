/*using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.CartItem.DTO;
namespace WebUser.features.CartItem.functions
{
    public class GetAllAttrValue
    {
        //input
        public class GetAllCartItemsQuery : IRequest<ICollection<CartItemDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllCartItemsQuery, ICollection<CartItemDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper _mapper;
            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                _mapper = mapper;
            }
            public async Task<ICollection<CartItemDTO>> Handle(GetAllCartItemsQuery request, CancellationToken cancellationToken)
            {
                var CartItems = await dbcontext.CartItems.ToListAsync(cancellationToken: cancellationToken);
                var results = _mapper.Map<ICollection<CartItemDTO>>(CartItems);
                return results;
            }
        }
    }
}
*/
