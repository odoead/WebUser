using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Coupon.DTO;
using WebUser.features.Product.Exceptions;
using WebUser.shared;
using E = WebUser.Domain.entities;

namespace WebUser.features.Coupon.Functions
{
    public class CreateCoupon
    {
        //input
        public class CreateCouponCommand : IRequest<CouponDTO>
        {
            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public double? DiscountVal { get; set; }
            public int DiscountPercent { get; set; }
            public int ProductId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateCouponCommand, CouponDTO>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<CouponDTO> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
            {
                var product =
                    await dbcontext.Products.FirstOrDefaultAsync(q => q.ID == request.ProductId, cancellationToken: cancellationToken)
                    ?? throw new ProductNotFoundException(request.ProductId);
                var coupon = new E.Coupon
                {
                    ActiveFrom = request.ActiveFrom,
                    ActiveTo = request.ActiveTo,
                    Code = CodeGenerator.GenerateCode(5),
                    CreatedAt = DateTime.UtcNow,
                    IsActivated = false,
                    Product = product,
                };
                if (request.DiscountVal > 0)
                {
                    coupon.DiscountVal = (double)request.DiscountVal;
                }
                if (request.DiscountPercent > 0)
                {
                    coupon.DiscountPercent = request.DiscountPercent;
                }
                if (
                    !await dbcontext.Coupons.AnyAsync(
                        q =>
                            q.ActiveFrom == coupon.ActiveFrom
                            && q.ActiveTo == coupon.ActiveTo
                            && q.DiscountVal == coupon.DiscountVal
                            && q.DiscountPercent == coupon.DiscountPercent
                            && q.Code == coupon.Code
                            && q.CreatedAt == coupon.CreatedAt
                            && q.IsActivated == coupon.IsActivated,
                        cancellationToken: cancellationToken
                    )
                )
                {
                    await dbcontext.Coupons.AddAsync(coupon, cancellationToken);
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
                var results = mapper.Map<CouponDTO>(coupon);
                return results;
            }
        }
    }
}
