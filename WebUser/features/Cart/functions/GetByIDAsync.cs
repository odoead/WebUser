using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Cart.DTO;
using WebUser.features.Cart.Exceptions;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Cart.functions
{
    public class GetByIDAttrValueAsync
    {
        //input
        public class GetByIDCartQuery : IRequest<CartDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByIDCartQuery, CartDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<CartDTO> Handle(GetByIDCartQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Cart.IsExistsAsync(new ObjectID<E.Cart>(request.Id)))
                {
                    var name = await _repoWrapper.Cart.GetByIdAsync(new ObjectID<E.Cart>(request.Id));
                    var results = _mapper.Map<CartDTO>(name);
                    return results;
                }
                else
                    throw new CartNotFoundException(request.Id);
            }
        }
    }
}
