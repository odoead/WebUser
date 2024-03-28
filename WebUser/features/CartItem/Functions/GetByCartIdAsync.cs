using AutoMapper;
using MediatR;
using WebUser.features.Cart.Exceptions;
using WebUser.features.CartItem.DTO;
using WebUser.features.CartItem.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.CartItem.Functions
{
    public class GetByCartIdAsync
    {//input
        public class GetByCartIDQuery : IRequest<ICollection<CartItemDTO>>
        {
            public int CartId { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByCartIDQuery, ICollection<CartItemDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<CartItemDTO>> Handle(GetByCartIDQuery request, CancellationToken cancellationToken)
            {
                if (!await _repoWrapper.Cart.IsExistsAsync(new ObjectID<E.Cart>(request.CartId)))
                { 
                    throw new CartNotFoundException(request.CartId);
                }
                var cartItems = await _repoWrapper.CartItem.GetByCartIdAsync(new ObjectID<E.Cart>(request.CartId));
                if (cartItems == null)
                {
                    throw new CartItemNotFoundException(-1);
                }
                return _mapper.Map<ICollection<CartItemDTO>>(cartItems);
                
            }
        }
    }
}
