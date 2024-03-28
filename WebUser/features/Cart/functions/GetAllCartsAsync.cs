using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Cart.DTO;
using WebUser.features.Category.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Cart.functions
{
    public class GetAllAttrValueAsync
    {
        //input
        public class GetAllCartsQuery : IRequest<ICollection<CartDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllCartsQuery, ICollection<CartDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper repoWrapper, IMapper mapper)
            {
                _repoWrapper = repoWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<CartDTO>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
            {
                var carts = await _repoWrapper.Cart.GetAllAsync();
                var results = _mapper.Map<ICollection<CartDTO>>(carts);
                return results;
            }
        }

    }
}
