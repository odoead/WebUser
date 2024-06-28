using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.discount.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.discount.Functions
{
    public class GetAllDiscounts
    {
        //input
        public class GetAllDiscountsQuery : IRequest<ICollection<DiscountDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllDiscountsQuery, ICollection<DiscountDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<DiscountDTO>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
            {
                var coupons = await dbcontext.Discounts.ToListAsync(cancellationToken: cancellationToken);
                var results = mapper.Map<ICollection<DiscountDTO>>(coupons);
                return PagedList<DiscountDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
