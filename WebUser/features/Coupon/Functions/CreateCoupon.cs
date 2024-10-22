using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Coupon.DTO;
using WebUser.features.Product.DTO;
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
            public double DiscountVal { get; set; }
            public int? DiscountPercent { get; set; }
            public int ProductId { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateCouponCommand, CouponDTO>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
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
                    DiscountVal = request.DiscountVal > 0 ? (double)request.DiscountVal : 0,
                    DiscountPercent = request.DiscountPercent > 0 ? request.DiscountPercent : 0,
                };

                await dbcontext.Coupons.AddAsync(coupon, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);

                var results = new CouponDTO
                {
                    ID = coupon.ID,
                    ActiveFrom = coupon.ActiveFrom,
                    ActiveTo = coupon.ActiveTo,
                    Code = coupon.Code,
                    CreatedAt = coupon.CreatedAt,
                    DiscountPercent = coupon.DiscountPercent,
                    DiscountVal = coupon.DiscountVal,
                    IsActivated = coupon.IsActivated,
                    Product = new ProductMinDTO
                    {
                        ID = coupon.Product.ID,
                        Name = coupon.Product.Name,
                        Price = coupon.Product.Price,
                    },
                    IsActive = E.Coupon.IsActive(coupon),
                    OrderID = null,
                };
                return results;
            }
        }
    }
}
