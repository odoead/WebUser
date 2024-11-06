namespace WebUser.features.Promotion_TODO.Functions;

using System.Threading;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Promotion.Exceptions;
using WebUser.features.Promotion_TODO.DTO;
using WebUser.shared.extentions;
using WebUser.shared.RepoWrapper;

public class UpdatePromotion
{
    public class UpdatePromotionCommand : IRequest
    {
        public int Id { get; set; }
        public JsonPatchDocument<UpdatePromotionDTO> PatchDoc { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<UpdatePromotionCommand>
    {
        private readonly DB_Context dbcontext;
        private readonly IServiceWrapper service;

        public async Task Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
        {
            if (request.PatchDoc == null)
            {
                return;
            }

            var promotion =
                await dbcontext
                    .Promotions.Include(p => p.Categories)
                    .Include(p => p.ConditionProducts)
                    .Include(p => p.AttributeValues)
                    .Include(p => p.ActionProducts)
                    .FirstOrDefaultAsync(p => p.ID == request.Id, cancellationToken: cancellationToken)
                ?? throw new PromotionNotFoundException(request.Id);

            var updatePromotion = new UpdatePromotionDTO
            {
                Name = promotion.Name,
                Description = promotion.Description,
                ActiveFrom = promotion.ActiveFrom,
                ActiveTo = promotion.ActiveTo,
                CategoriesIds = promotion.Categories.Select(c => c.CategoryID).ToList(),
                AttributeValueIds = promotion.AttributeValues.Select(av => av.AttributeValueID).ToList(),
                ProductsForPromotionIds = promotion.ConditionProducts.Select(p => p.ProductID).ToList(),
                ProductsForPromotionPromids = promotion.ActionProducts.Select(pp => pp.ProductID).ToList(),
                IsFirstOrder = promotion.IsFirstOrder,
                DiscountVal = promotion.DiscountVal,
                DiscountPercent = promotion.DiscountPercent,
                BuyQuantity = promotion.BuyQuantity,
                GetQuantity = promotion.GetQuantity,
                MinPay = promotion.MinPay,
                PointsValue = promotion.PointsValue,
                PointsPercent = promotion.PointsPercent,
                PointsExpireDays = promotion.PointsExpireDays,
            };

            request.PatchDoc.ApplyTo(updatePromotion);

            promotion.Name = updatePromotion.Name;
            promotion.Description = updatePromotion.Description;
            promotion.ActiveFrom = updatePromotion.ActiveFrom;
            promotion.ActiveTo = updatePromotion.ActiveTo;
            promotion.IsFirstOrder = updatePromotion.IsFirstOrder;
            promotion.DiscountVal = updatePromotion.DiscountVal;
            promotion.DiscountPercent = updatePromotion.DiscountPercent;
            promotion.BuyQuantity = updatePromotion.BuyQuantity;
            promotion.GetQuantity = updatePromotion.GetQuantity;
            promotion.MinPay = updatePromotion.MinPay;
            promotion.PointsValue = updatePromotion.PointsValue;
            promotion.PointsPercent = updatePromotion.PointsPercent;
            promotion.PointsExpireDays = updatePromotion.PointsExpireDays;

            /*if (updatePromotion.CategoriesIds != null)
            {
                promotion.Categories.Clear();
                foreach (var categoryId in updatePromotion.CategoriesIds)
                {
                    var category = await dbcontext.Categories.FirstOrDefaultAsync(c => c.ID == categoryId, cancellationToken);
                    if (category != null)
                    {
                        promotion.Categories.Add(
                            new PromotionCategory
                            {
                                Promotion = promotion,
                                PromotionID = promotion.ID,
                                Category = category,
                                CategoryID = category.ID,
                            }
                        );
                    }
                }
            }
            ===
             */
            await ManyToManyEntitiesUpdater.UpateManyToManyRelationsAsync<Promotion, Category, PromotionCategory>(
                dbcontext,
                promotion,
                promotion.Categories,
                updatePromotion.CategoriesIds,
                (prom, categ) =>
                    new PromotionCategory
                    {
                        Promotion = prom,
                        PromotionID = prom.ID,
                        Category = categ,
                        CategoryID = categ.ID,
                    },
                async ids =>
                    await dbcontext.Categories.Where(av => ids.Contains(av.ID)).ToListAsync(cancellationToken)
            );

            await ManyToManyEntitiesUpdater.UpateManyToManyRelationsAsync<Promotion, AttributeValue, PromotionAttrValue>(
                dbcontext,
                promotion,
                promotion.AttributeValues,
                updatePromotion.AttributeValueIds,
                (prom, attrValue) =>
                    new PromotionAttrValue
                    {
                        Promotion = prom,
                        PromotionID = prom.ID,
                        AttributeValue = attrValue,
                        AttributeValueID = attrValue.ID,
                    },
                async ids => await dbcontext.AttributeValues.Where(av => ids.Contains(av.ID)).ToListAsync(cancellationToken)
            );

            await ManyToManyEntitiesUpdater.UpateManyToManyRelationsAsync<Promotion, Product, PromotionProduct>(
                dbcontext,
                promotion,
                promotion.ConditionProducts,
                updatePromotion.ProductsForPromotionIds,
                (prom, prodct) =>
                    new PromotionProduct
                    {
                        Promotion = prom,
                        PromotionID = prom.ID,
                        Product = prodct,
                        ProductID = prodct.ID,
                    },
                async ids => await dbcontext.Products.Where(av => ids.Contains(av.ID)).ToListAsync(cancellationToken)
            );

            await ManyToManyEntitiesUpdater.UpateManyToManyRelationsAsync<Promotion, Product, PromotionPromProduct>(
                dbcontext,
                promotion,
                promotion.ActionProducts,
                updatePromotion.ProductsForPromotionPromids,
                (prom, prodct) =>
                    new PromotionPromProduct
                    {
                        Promotion = prom,
                        PromotionID = prom.ID,
                        Product = prodct,
                        ProductID = prodct.ID,
                    },
                async ids => await dbcontext.Products.Where(av => ids.Contains(av.ID)).ToListAsync(cancellationToken)
            );

            await dbcontext.SaveChangesAsync(cancellationToken);
        }
    }
}
