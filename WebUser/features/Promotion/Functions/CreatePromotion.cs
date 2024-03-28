using AutoMapper;
using MediatR;
using WebUser.features.Promotion.DTO;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.Functions
{
    public class CreatePromotion
    {
        //input
        public class CreatePromotionCommand : IRequest<PromotionDTO>
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public E.Category Category { get; set; }
            public ICollection<E.AttributeName> AttributeNames { get; set; }

            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public double DiscountVal { get; set; }
            public float DiscountPercent { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreatePromotionCommand, PromotionDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<PromotionDTO> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
            {
                var promotion = new E.Promotion
                {
                    ActiveFrom = request.ActiveFrom,
                    ActiveTo = request.ActiveTo,
                    AttributeNames = request.AttributeNames,
                    Description = request.Description,
                    Category = request.Category,
                    CategoryId = request.Category.ID,
                    ID = request.Category.ID,
                    CreatedAt = DateTime.UtcNow,
                    DiscountPercent = request.DiscountPercent,
                    DiscountVal = request.DiscountVal,
                    Name = request.Name

                };
                _repoWrapper.Promotion.Create(promotion);
                await _repoWrapper.SaveAsync();
                var results = _mapper.Map<PromotionDTO>(promotion);
                return results;
            }
        }

    }
}
