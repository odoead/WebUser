using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Promotion.DTO;
using WebUser.features.Promotion.Exceptions;

namespace WebUser.features.Promotion.Functions
{
    public class GetPromotionByID
    {
        //input
        public class GetByOrderProdIDQuery : IRequest<PromotionDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetByOrderProdIDQuery, PromotionDTO>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<PromotionDTO> Handle(GetByOrderProdIDQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Promotions.AnyAsync(q => q.ID == request.Id, cancellationToken: cancellationToken))
                {
                    var promo = await dbcontext.Promotions.FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken);
                    var results = mapper.Map<PromotionDTO>(promo);
                    return results;
                }
                throw new PromotionNotFoundException(request.Id);
            }
        }
    }
}
