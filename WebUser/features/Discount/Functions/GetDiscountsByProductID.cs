using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.discount.DTO;
using WebUser.features.discount.Exceptions;
using WebUser.features.Product.Exceptions;

namespace WebUser.features.discount.Functions
{
    public class GetDiscountsByProductID
    {
        //input
        public class GetDiscountByProductIDQuery : IRequest<ICollection<DiscountDTO>>
        {
            public int ProductId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetDiscountByProductIDQuery, ICollection<DiscountDTO>>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<DiscountDTO>> Handle(GetDiscountByProductIDQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Products.AnyAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken))
                    throw new ProductNotFoundException(request.ProductId);
                var Discount =
                    await dbcontext.Discounts.Where(q => q.Product.ID == request.ProductId).ToListAsync(cancellationToken: cancellationToken)
                    ?? throw new DiscountNotFoundException(-1);
                var results = mapper.Map<ICollection<DiscountDTO>>(Discount);
                return results;
            }
        }
    }
}
