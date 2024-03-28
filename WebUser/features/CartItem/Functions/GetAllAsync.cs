using AutoMapper;
using MediatR;
using WebUser.features.CartItem.DTO;
using WebUser.features.Category.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.CartItem.functions
{
    public class GetAllAttrValueAsync
    {
        //input
        public class GetAllCartItemsQuery : IRequest<ICollection<CartItemDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllCartItemsQuery, ICollection<CartItemDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper repoWrapper, IMapper mapper)
            {
                _repoWrapper = repoWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<CartItemDTO>> Handle(GetAllCartItemsQuery request, CancellationToken cancellationToken)
            {
                var CartItems = await _repoWrapper.CartItem.GetAllAsync();
                var results = _mapper.Map<ICollection<CartItemDTO>>(CartItems);
                return results;
            }
        }

    }
}
