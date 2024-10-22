using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Promotion.DTO;
using WebUser.features.Promotion.Exceptions;
using E = WebUser.Domain.entities;

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

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<PromotionDTO> Handle(GetByOrderProdIDQuery request, CancellationToken cancellationToken)
            {
                var promo =
                    await dbcontext
                        .Promotions.Include(p => p.PromProducts)
                        .ThenInclude(pp => pp.Product)
                        .Include(p => p.Products)
                        .ThenInclude(pp => pp.Product)
                        .Include(p => p.Categories)
                        .ThenInclude(c => c.Category)
                        .Include(p => p.AttributeValues)
                        .ThenInclude(av => av.AttributeValue)
                        .ThenInclude(an => an.AttributeName)
                        .FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken)
                    ?? throw new PromotionNotFoundException(request.Id);

                var promotionDTO = new PromotionDTO
                {
                    ID = promo.ID,
                    IsActive = E.Promotion.IsActive(promo),
                    Name = promo.Name,
                    Description = promo.Description,
                    CreatedAt = promo.CreatedAt,
                    ActiveFrom = promo.ActiveFrom,
                    ActiveTo = promo.ActiveTo,

                    // Actions
                    DiscountVal = promo.DiscountVal,
                    DiscountPercent = promo.DiscountPercent,
                    GetQuantity = promo.GetQuantity,
                    PointsValue = promo.PointsValue,
                    PointsPercent = promo.PointsPercent,
                    PointsExpireDays = promo.PointsExpireDays,

                    // Conditions
                    MinPay = promo.MinPay,
                    BuyQuantity = promo.BuyQuantity,
                    IsFirstOrder = promo.IsFirstOrder,

                    // Map PromoProducts
                    PromoProducts = promo
                        .PromProducts.Select(pp => new ProductMinDTO
                        {
                            ID = pp.Product.ID,
                            Name = pp.Product.Name,
                            Price = pp.Product.Price,
                        })
                        .ToList(),

                    // Map Categories
                    Categories = promo.Categories.Select(c => new CategoryMinDTO { ID = c.Category.ID, Name = c.Category.Name }).ToList(),

                    // Map Products
                    Products = promo
                        .Products.Select(p => new ProductMinDTO
                        {
                            ID = p.ProductID,
                            Name = p.Product.Name,
                            Price = p.Product.Price,
                        })
                        .ToList(),

                    // Map AttributeValues
                    AttributeValues = promo
                        .AttributeValues.GroupBy(av => av.AttributeValue.AttributeName)
                        .Select(g => new AttributeNameValueDTO
                        {
                            AttributeName = new AttributeNameMinDTO { ID = g.Key.ID, Name = g.Key.Name },
                            Attributes = g.Select(av => new AttributeValueDTO { ID = av.AttributeValue.ID, Value = av.AttributeValue.Value })
                                .ToList(),
                        })
                        .ToList(),
                };

                return promotionDTO;
            }
        }
    }
}
