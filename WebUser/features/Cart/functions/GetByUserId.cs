using AutoMapper;
using MediatR;
using WebUser.features.Cart.DTO;
using WebUser.features.Cart.Exceptions;
using WebUser.shared.RepoWrapper;
using WebUser.shared;
using E=WebUser.Domain.entities;

namespace WebUser.features.Cart.functions
{
    public class GetByUserId
    {
        //input
        public class GetByUserIDCartQuery : IRequest<CartDTO>
        {
            public int UserId { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByUserIDCartQuery, CartDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<CartDTO> Handle(GetByUserIDCartQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.user.IsExistsAsync(new ObjectID<E.User>(request.UserId)))
                {
                    var cart = await _repoWrapper.Cart.GetByUserIdAsync(new ObjectID<E.User>(request.UserId));
                    var results = _mapper.Map<CartDTO>(cart);
                    return results;
                }
                throw new CartNotFoundException(request.UserId);
            }
        }
    }
}
