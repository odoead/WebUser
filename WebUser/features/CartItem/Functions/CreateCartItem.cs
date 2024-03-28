/*using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.features.Cart.DTO;
using WebUser.shared.RepoWrapper;
using WebUser.features.CartItem.DTO;

namespace WebUser.features.CartItem.functions
{
    public class CreateCartItem
    {
        //input
        public class CreateCartItemCommand : IRequest<CartItemDTO>
        {
            public int ID { get; set; }
            public E.Cart Cart { get; set; }
            public int Amount { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreateCartItemCommand, CartItemDTO>
        {
            private IRepoWrapper _RepoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper RepoWrapper, IMapper mapper)
            {
                _RepoWrapper = RepoWrapper;
                _mapper = mapper;
            }

            public async Task<CartItemDTO> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
            {
                var cartItem = new E.CartItem
                {
                   ID = request.ID,
                   Amount=request.Amount,
                   Cart = request.Cart,
                    CartId=request.Cart.ID
                    
                };
                _RepoWrapper.CartItem.Create(cartItem);
                await _RepoWrapper.SaveAsync();
                var results = _mapper.Map<CartItemDTO>(cartItem);
                return results;
            }
        }

    }
}
*/