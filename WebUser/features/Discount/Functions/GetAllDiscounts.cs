using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.discount.DTO;
using WebUser.features.Discount;
using WebUser.features.Product.DTO;
using WebUser.shared.RequestForming.features;
using E = WebUser.Domain.entities;

namespace WebUser.features.discount.Functions
{
    public class GetAllDiscounts
    {
        //input
        public class GetAllDiscountsQuery : IRequest<PagedList<DiscountDTO>>
        {
            public DiscountRequestParameters Parameters { get; set; }

            public GetAllDiscountsQuery(DiscountRequestParameters parameters)
            {
                Parameters = parameters;
            }
        }

        //handler
        public class Handler : MediatR.IRequestHandler<GetAllDiscountsQuery, PagedList<DiscountDTO>>
        {
            private readonly DB_Context dbcontext;


            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<PagedList<DiscountDTO>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
            {
                var data = dbcontext.Discounts.Include(q => q.Product).AsQueryable();
                var paged = await data.Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                    .Take(request.Parameters.PageSize)
                    .ToListAsync(cancellationToken);

                var discountDTOs = new List<DiscountDTO>();
                foreach (var discount in paged)
                {
                    var discountDto = new DiscountDTO
                    {
                        ID = discount.ID,
                        ActiveFrom = discount.ActiveFrom,
                        ActiveTo = discount.ActiveTo,
                        CreatedAt = discount.CreatedAt,
                        DiscountPercent = discount.DiscountPercent,
                        DiscountVal = discount.DiscountVal,
                        Product = new ProductMinDTO
                        {
                            ID = discount.Product.ID,
                            Name = discount.Product.Name,
                            Price = discount.Product.Price,
                        },
                        IsActive = E.Discount.IsActive(discount),
                    };

                    discountDTOs.Add(discountDto);
                }

                var pagedList = PagedList<DiscountDTO>.PaginateList(
                    source: discountDTOs,
                    totalCount: await data.CountAsync(cancellationToken: cancellationToken),
                    pageNumber: request.Parameters.PageNumber,
                    pageSize: request.Parameters.PageSize
                );

                return pagedList;
            }
        }
    }
}
