using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.discount.DTO;
using WebUser.features.Product.Exceptions;
using E = WebUser.Domain.entities;

namespace WebUser.features.Product.Functions
{
    public class AddDiscountToProduct
    {
        //input
        public class AddDiscountToProductCommand : IRequest<DiscountDTO>
        {
            public int ProductId { get; set; }
            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public double DiscountValue { get; set; }
            public int DiscountPercent { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<AddDiscountToProductCommand, DiscountDTO>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<DiscountDTO> Handle(AddDiscountToProductCommand request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Discounts.AnyAsync(q => q.Product.ID == request.ProductId, cancellationToken: cancellationToken))
                {
                    return mapper.Map<DiscountDTO>(
                        dbcontext.Discounts.FirstOrDefaultAsync(q => q.Product.ID == request.ProductId, cancellationToken: cancellationToken)
                    );
                }
                var product =
                    await dbcontext.Products.FirstOrDefaultAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken)
                    ?? throw new ProductNotFoundException(request.ProductId);
                var discount = new E.Discount
                {
                    ActiveFrom = request.ActiveFrom,
                    ActiveTo = request.ActiveTo,
                    CreatedAt = DateTime.UtcNow,
                    Product = product,
                    DiscountVal = 0,
                    DiscountPercent = 0,
                };
                if (request.DiscountValue > 0)
                {
                    discount.DiscountVal = (double)request.DiscountValue;
                }
                if (request.DiscountPercent > 0)
                {
                    discount.DiscountPercent = request.DiscountPercent;
                }
                if (
                    !await dbcontext.Discounts.AnyAsync(
                        q =>
                            q.ActiveFrom == discount.ActiveFrom
                            && q.ActiveTo == discount.ActiveTo
                            && q.DiscountPercent == discount.DiscountPercent
                            && q.Product == discount.Product
                            && q.CreatedAt == discount.CreatedAt,
                        cancellationToken: cancellationToken
                    )
                )
                {
                    await dbcontext.Discounts.AddAsync(discount, cancellationToken);
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
                var results = mapper.Map<DiscountDTO>(discount);
                return results;
            }
        }
    }
}
