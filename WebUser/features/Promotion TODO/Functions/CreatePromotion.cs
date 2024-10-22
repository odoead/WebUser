using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.DTO;
using WebUser.features.Product.DTO;
using WebUser.features.Promotion.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Promotion.Functions
{
    public class CreatePromotion
    {
        //input
        public class CreatePromotionCommand : IRequest<PromotionDTO>
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public List<int> CategoryIDs { get; set; }
            public List<int> AttributeValuesIDs { get; set; }
            public List<int> ProductIDsForPromotion { get; set; }
            public List<int> ProductIDsForPromotionProm { get; set; }
            public bool IsFirstOrder { get; set; }

            public double? DiscountVal { get; set; }

            [Range(0.01, 100, ErrorMessage = "Only 0.01-100 range allowed")]
            public int? DiscountPercent { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int buyQuantity { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int getQuantity { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int MinPay { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int? PointsValue { get; set; }

            [Range(1, 100, ErrorMessage = "Only 0.01-100 range allowed")]
            public int? PointsPercent { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int PointsExpireDays { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreatePromotionCommand, PromotionDTO>
        {
            private DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<PromotionDTO> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
            {
                var attributeValues = dbcontext
                    .AttributeValues.Include(q => q.AttributeName)
                    .Where(e => request.AttributeValuesIDs.Contains(e.ID))
                    .ToList();
                var categories = dbcontext.Categories.Where(e => request.CategoryIDs.Contains(e.ID)).ToList();
                var products = dbcontext.Products.Where(e => request.ProductIDsForPromotion.Contains(e.ID)).ToList();
                var promProducts = dbcontext.Products.Where(e => request.ProductIDsForPromotionProm.Contains(e.ID)).ToList();
                var promotion = new E.Promotion
                {
                    ActiveFrom = request.ActiveFrom,
                    ActiveTo = request.ActiveTo,
                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                    DiscountPercent = request.DiscountPercent,
                    DiscountVal = request.DiscountVal,
                    Name = request.Name,
                    BuyQuantity = request.buyQuantity,
                    GetQuantity = request.getQuantity,
                    MinPay = request.MinPay,
                    PointsExpireDays = request.PointsExpireDays,
                    PointsPercent = request.PointsPercent,
                    PointsValue = request.PointsValue,
                    IsFirstOrder = request.IsFirstOrder,
                };

                var promotionAttributeValues = attributeValues
                    .Select(attributeValue => new PromotionAttrValue
                    {
                        Promotion = promotion,
                        PromotionID = promotion.ID,
                        AttributeValue = attributeValue,
                        AttributeValueID = attributeValue.ID,
                    })
                    .ToList();
                promotion.AttributeValues = promotionAttributeValues;

                var promotionCategories = categories
                    .Select(category => new PromotionCategory
                    {
                        Promotion = promotion,
                        PromotionID = promotion.ID,
                        Category = category,
                        CategoryID = category.ID,
                    })
                    .ToList();
                promotion.Categories = promotionCategories;

                var promotionProducts = products
                    .Select(product => new PromotionProduct
                    {
                        Promotion = promotion,
                        PromotionID = promotion.ID,
                        Product = product,
                        ProductID = product.ID,
                    })
                    .ToList();
                promotion.Products = promotionProducts;

                var promotionPromProducts = promProducts
                    .Select(promProduct => new PromotionPromProduct
                    {
                        Promotion = promotion,
                        PromotionID = promotion.ID,
                        Product = promProduct,
                        ProductID = promProduct.ID,
                    })
                    .ToList();
                promotion.PromProducts = promotionPromProducts;

                await dbcontext.Promotions.AddAsync(promotion, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);

                var promotionDTO = new PromotionDTO
                {
                    ID = promotion.ID,
                    IsActive = E.Promotion.IsActive(promotion),
                    Name = promotion.Name,
                    Description = promotion.Description,
                    CreatedAt = promotion.CreatedAt,
                    ActiveFrom = promotion.ActiveFrom,
                    ActiveTo = promotion.ActiveTo,

                    // Actions
                    DiscountVal = promotion.DiscountVal,
                    DiscountPercent = promotion.DiscountPercent,
                    GetQuantity = promotion.GetQuantity,
                    PointsValue = promotion.PointsValue,
                    PointsPercent = promotion.PointsPercent,
                    PointsExpireDays = promotion.PointsExpireDays,

                    // Conditions
                    MinPay = promotion.MinPay,
                    BuyQuantity = promotion.BuyQuantity,
                    IsFirstOrder = promotion.IsFirstOrder,

                    // Map PromoProducts
                    PromoProducts = promotion
                        .PromProducts.Select(pp => new ProductMinDTO
                        {
                            ID = pp.Product.ID,
                            Name = pp.Product.Name,
                            Price = pp.Product.Price,
                        })
                        .ToList(),

                    // Map Categories
                    Categories = promotion.Categories.Select(c => new CategoryMinDTO { ID = c.Category.ID, Name = c.Category.Name }).ToList(),

                    // Map Products
                    Products = promotion
                        .Products.Select(p => new ProductMinDTO
                        {
                            ID = p.Product.ID,
                            Name = p.Product.Name,
                            Price = p.Product.Price,
                        })
                        .ToList(),

                    // Map AttributeValues
                    AttributeValues = promotion
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
