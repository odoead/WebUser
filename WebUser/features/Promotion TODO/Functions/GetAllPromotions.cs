using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Promotion.DTO;
using WebUser.features.Promotion_TODO;
using WebUser.shared.RequestForming.features;
using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.Functions
{
    public class GetAllPromotions
    {
        //input
        public class GetAllPromotionsQuery : IRequest<PagedList<PromotionDTO>>
        {
            public PromotionRequestParameters Parameters { get; set; }

            public GetAllPromotionsQuery(PromotionRequestParameters parameters)
            {
                Parameters = parameters;
            }
        }

        //handler
        public class Handler : IRequestHandler<GetAllPromotionsQuery, PagedList<PromotionDTO>>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<PagedList<PromotionDTO>> Handle(GetAllPromotionsQuery request, CancellationToken cancellationToken)
            {
                var data = dbcontext
                    .Promotions.Include(p => p.Categories)
                    .ThenInclude(q => q.Category)
                    .Include(p => p.Products)
                    .ThenInclude(q => q.Product)
                    .Include(p => p.AttributeValues)
                    .ThenInclude(q => q.AttributeValue)
                    .ThenInclude(q => q.AttributeName)
                    .Include(p => p.PromProducts)
                    .ThenInclude(q => q.Product)
                    .AsQueryable();

                var paginatedPromotions = await data.Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                    .Take(request.Parameters.PageSize)
                    .ToListAsync(cancellationToken);

                var promotionDTOs = paginatedPromotions
                    .Select(promo => new PromotionDTO
                    {
                        ID = promo.ID,
                        IsActive = E.Promotion.IsActive(promo),
                        Name = promo.Name,
                        Description = promo.Description,
                        CreatedAt = promo.CreatedAt,
                        ActiveFrom = promo.ActiveFrom,
                        ActiveTo = promo.ActiveTo,

                        PromoProducts = promo
                            .PromProducts.Select(pp => new ProductMinDTO
                            {
                                ID = pp.Product.ID,
                                Name = pp.Product.Name,
                                Price = pp.Product.Price,
                            })
                            .ToList(),

                        Categories = promo.Categories.Select(c => new CategoryMinDTO { ID = c.Category.ID, Name = c.Category.Name }).ToList(),

                        Products = promo
                            .Products.Select(p => new ProductMinDTO
                            {
                                ID = p.Product.ID,
                                Name = p.Product.Name,
                                Price = p.Product.Price,
                            })
                            .ToList(),

                        AttributeValues = promo
                            .AttributeValues.GroupBy(av => av.AttributeValue.AttributeName)
                            .Select(g => new AttributeNameValueDTO
                            {
                                AttributeName = new AttributeNameMinDTO { ID = g.Key.ID, Name = g.Key.Name },
                                Attributes = g.Select(av => new AttributeValueDTO { ID = av.AttributeValue.ID, Value = av.AttributeValue.Value })
                                    .ToList(),
                            })
                            .ToList(),

                        DiscountVal = promo.DiscountVal,
                        DiscountPercent = promo.DiscountPercent,
                        GetQuantity = promo.GetQuantity,
                        PointsValue = promo.PointsValue,
                        PointsPercent = promo.PointsPercent,
                        PointsExpireDays = promo.PointsExpireDays,
                        MinPay = promo.MinPay,
                        BuyQuantity = promo.BuyQuantity,
                        IsFirstOrder = promo.IsFirstOrder,
                    })
                    .ToList();

                var totalCount = await data.CountAsync(cancellationToken);

                // Create paginated result using the manually mapped list
                var pagedList = PagedList<PromotionDTO>.PaginateList(
                    source: promotionDTOs,
                    totalCount: totalCount,
                    pageNumber: request.Parameters.PageNumber,
                    pageSize: request.Parameters.PageSize
                );

                return pagedList;
            }
        }
    }
}
