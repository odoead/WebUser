using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.CartItem.DTO;
using WebUser.features.CartItem.Exceptions;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.CartItem.functions
{
    public class GetByIDAttrValueAsync
    {
        //input
        public class GetByIDCartItemQuery : IRequest<CartItemDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByIDCartItemQuery, CartItemDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<CartItemDTO> Handle(GetByIDCartItemQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.CartItem.IsExistsAsync(new ObjectID<E.CartItem> (request.Id)))
                {
                    var cartItem = await _repoWrapper.CartItem.GetByIdAsync(new ObjectID<E.CartItem>(request.Id));
                    var results = _mapper.Map<CartItemDTO>(cartItem);
                    return results;
                }
                throw new CartItemNotFoundException(request.Id);
            }
        }
    }
}
