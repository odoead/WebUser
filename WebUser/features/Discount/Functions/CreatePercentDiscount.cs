/*using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.shared.RepoWrapper;
using WebUser.features.discount.DTO;

namespace WebUser.features.discount.Functions
{
    public class CreatePercentDiscountForProduct
    {
        //input
        public class CreatePercentDiscountCommand : IRequest<DiscountDTO>
        {
            public int ID { get; set; }
            public E.Product Product { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public float DiscountPercent { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreatePercentDiscountCommand, DiscountDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<DiscountDTO> Handle(CreatePercentDiscountCommand request, CancellationToken cancellationToken)
            {
                var disc = new E.Discount
                {
                    CreatedAt = DateTime.Now,
                    DiscountPercent = request.DiscountPercent,
                    ActiveTo = request.ActiveTo,
                    ActiveFrom = request.ActiveFrom,
                    Product = request.Product,
                    
                };
                _repoWrapper.Discount.Create(disc);
                var results = _mapper.Map<DiscountDTO>(disc);
                return results;
            }
        }

    }
}
*/