using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.features.Promotion.DTO;
using WebUser.features.Promotion.Exceptions;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Promotion.Functions
{
    public class GetPromotionByIDAsync
    {
        //input
        public class GetByOrderProdIDQuery : IRequest<PromotionDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByOrderProdIDQuery, PromotionDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<PromotionDTO> Handle(GetByOrderProdIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Promotion.IsExistsAsync(request.Id))
                {
                    var categories = await _repoWrapper.Promotion.GetByIdAsync(request.Id);
                    var results = _mapper.Map<PromotionDTO>(categories);
                    return results;
                }
                throw new PromotionNotFoundException(request.Id);
            }
        }
    }
}
