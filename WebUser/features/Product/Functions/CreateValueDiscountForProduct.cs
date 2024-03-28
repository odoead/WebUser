using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.discount.DTO;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Discount.Functions
{
    public class CreateValueDiscountForProduct
    {
        //input
        public class CreatePercentDiscountCommand : IRequest<DiscountDTO>
        {
            public int ID { get; set; }
            public E.Product Product { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public double DiscountValue { get; set; }
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
                    DiscountVal = request.DiscountValue,
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
