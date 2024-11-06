namespace WebUser.features.Product.Functions;

using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Coupon.DTO;
using WebUser.features.Discount.DTO;
using WebUser.features.Image.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Product.Exceptions;
using WebUser.features.Promotion_TODO.DTO;
using WebUser.shared.RepoWrapper;

public class GetProductPage
{
    //input
    public class GetProductPageByIDQuery : IRequest<DisplayProductPageDTO>
    {
        public int Id { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<GetProductPageByIDQuery, DisplayProductPageDTO>
    {
        private readonly DB_Context dbcontext;
        private readonly IServiceWrapper service;

        public Handler(DB_Context context, IServiceWrapper service)
        {
            dbcontext = context;
            this.service = service;
        }

        public async Task<DisplayProductPageDTO> Handle(GetProductPageByIDQuery request, CancellationToken cancellationToken)
        {
            var product =
                await dbcontext
                    .Products.Include(p => p.AttributeValues)
                    .ThenInclude(av => av.AttributeValue)
                    .ThenInclude(av => av.AttributeName)
                    .Include(p => p.Images)
                    .Include(p => p.Discounts)
                    .Include(p => p.Coupons)
                    .Include(p => p.Promotions)
                    .ThenInclude(p => p.Promotion)
                    .FirstOrDefaultAsync(p => p.ID == request.Id, cancellationToken) ?? throw new ProductNotFoundException(request.Id);

            var appliedDiscounts = await service.Pricing.ApplyDiscountAsync(new List<int> { request.Id });

            var attributeNameValues = GetAttributeNameValues(product);

            var parentRoute = await GetProductCategoriesRoute(product);

            var productPageDTO = new DisplayProductPageDTO
            {
                ID = product.ID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                AfterDiscountPrice = service.Product.CalculatePriceWithCumulativeDiscounts(product.ID, product.Price, appliedDiscounts),
                Stock = product.Stock - product.ReservedStock,
                ReservedStock = product.ReservedStock,
                IsPurchasable = Product.IsPurchasable(product, 1),
                Images = product.Images.Select(img => new ImageDTO { ID = img.ID, ImageContent = img.ImageContent }).ToList(),
                AttributeValues = attributeNameValues,
                Discounts = product
                    .Discounts.Select(disc => new DiscountMinDTO
                    {
                        DiscountPercent = disc.DiscountPercent,
                        ID = disc.ID,
                        DiscountVal = disc.DiscountVal,
                        IsActive = Discount.IsActive(disc),
                    })
                    .ToList(),
                Coupons = product
                    .Coupons.Select(coupon => new CouponMinDTO
                    {
                        ID = coupon.ID,
                        DiscountPercent = coupon.DiscountPercent,
                        DiscountVal = coupon.DiscountVal,
                        IsActive = coupon.IsActivated,
                    })
                    .ToList(),
                Promotions = product
                    .Promotions.Select(promotionLink => new PromotionMinDTO { ID = promotionLink.Promotion.ID, Name = promotionLink.Promotion.Name })
                    .ToList(),
                RouteCategories = parentRoute.Select(category => new CategoryMinDTO { ID = category.ID, Name = category.Name }).ToList(),
            };

            return productPageDTO;
        }

        private async Task<List<Category>> GetProductCategoriesRoute(Product product)
        {
            var atributeIDs = product.AttributeValues.Select(q => q.AttributeValueID).ToList();
            var category = dbcontext
                .Categories.Include(q => q.Attributes)
                .ThenInclude(anc => anc.AttributeName)
                .ThenInclude(an => an.AttributeValues)
                .Where(category =>
                    category
                        .Attributes.SelectMany(a => a.AttributeName.AttributeValues)
                        .Where(av => atributeIDs.Contains(av.ID))
                        .Select(av => av.ID)
                        .Distinct()
                        .Count() == atributeIDs.Count
                )
                .FirstOrDefault();

            var parentRoute = new List<Category>();
            parentRoute.AddRange(await service.Category.GetParentCategoriesLine(category.ID));
            parentRoute.Add(category);
            return parentRoute;
        }

        private List<AttributeNameValueDTO> GetAttributeNameValues(Product product) => product
                .AttributeValues.GroupBy(av => av.AttributeValue.AttributeName)
                .Select(g => new AttributeNameValueDTO
                {
                    AttributeName = new AttributeNameMinDTO { ID = g.Key.ID, Name = g.Key.Name },
                    Attributes = g.Select(av => new AttributeValueDTO { ID = av.AttributeValue.ID, Value = av.AttributeValue.Value }).ToList(),
                })
                .ToList();
    }
}
