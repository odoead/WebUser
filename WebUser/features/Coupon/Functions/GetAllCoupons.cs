using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Coupon.DTO;
using WebUser.features.Product.DTO;
using WebUser.shared.RequestForming.features;
using E = WebUser.Domain.entities;

namespace WebUser.features.Coupon.Functions
{
    public class GetAllCoupons
    {
        //input
        public class GetAllCouponAsyncQuery : IRequest<PagedList<CouponDTO>>
        {
            public CouponRequestParameters Parameters { get; set; }

            public GetAllCouponAsyncQuery(CouponRequestParameters parameters)
            {
                Parameters = parameters;
            }
        }

        //handler
        public class Handler : IRequestHandler<GetAllCouponAsyncQuery, PagedList<CouponDTO>>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<PagedList<CouponDTO>> Handle(GetAllCouponAsyncQuery request, CancellationToken cancellationToken)
            {
                var data = dbcontext.Coupons.Include(q => q.Product).Include(q => q.Order).AsQueryable();
                var pagedCoupons = await data.Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                    .Take(request.Parameters.PageSize)
                    .ToListAsync(cancellationToken);

                var couponDTOs = new List<CouponDTO>();

                foreach (var coupon in pagedCoupons)
                {
                    var couponDTO = new CouponDTO
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
                        OrderID = coupon.OrderID,
                    };

                    couponDTOs.Add(couponDTO);
                }

                var pagedList = PagedList<CouponDTO>.PaginateList(
                    source: couponDTOs,
                    totalCount: await data.CountAsync(cancellationToken: cancellationToken),
                    pageNumber: request.Parameters.PageNumber,
                    pageSize: request.Parameters.PageSize
                );

                return pagedList;
            }
        }
    }
}
