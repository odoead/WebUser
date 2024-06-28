using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.Promotion.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Promotion.Functions
{
    public class GetAllPromotions
    {
        //input
        public class GetAllOrderProdsAsyncQuery : IRequest<ICollection<PromotionDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllOrderProdsAsyncQuery, ICollection<PromotionDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<PromotionDTO>> Handle(GetAllOrderProdsAsyncQuery request, CancellationToken cancellationToken)
            {
                var categories = await dbcontext.Promotions.ToListAsync(cancellationToken: cancellationToken);
                var results = mapper.Map<ICollection<PromotionDTO>>(categories);
                return PagedList<PromotionDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
