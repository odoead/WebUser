using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using WebUser.features.discount.DTO;
using WebUser.features.discount.Exceptions;

namespace WebUser.features.discount.Functions
{
    public class GetDiscountByIDAsync
    {
        //input
        public class GetByIDQuery : IRequest<DiscountDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByIDQuery, DiscountDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<DiscountDTO> Handle(GetByIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Discount.IsExistsAsync(new ObjectID<E.Discount>(request.Id)))
                {
                    var discount = await _repoWrapper.Discount.GetByIdAsync(new ObjectID<E.Discount>(request.Id));
                    var results = _mapper.Map<DiscountDTO>(discount);
                    return results;
                }
                throw new DiscountNotFoundException(request.Id);
            }
        }
    }
}
