namespace WebUser.features.Product.Functions;

using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using WebUser.features.Product.DTO;
using WebUser.shared.extentions;

public class UpdateProduct
{
    //input
    public class UpdateProductCommand : IRequest
    {
        public int Id { get; set; }
        public JsonPatchDocument<UpdateProductDTO> PatchDoc { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<UpdateProductCommand>
    {
        private readonly DB_Context dbcontext;

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (request.PatchDoc == null)
            {
                return;
            }
            var product =
                await dbcontext
                    .Products.Include(c => c.AttributeValues)
                    .ThenInclude(q => q.AttributeValue)
                    .FirstOrDefaultAsync(c => c.ID == request.Id, cancellationToken: cancellationToken)
                ?? throw new CategoryNotFoundException(request.Id);
            var prevStock = product.Stock;
            var updateProduct = new UpdateProductDTO
            {
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                AttributeValueIds = product.AttributeValues.Select(q => q.AttributeValueID).ToList(),
            };
            request.PatchDoc.ApplyTo(updateProduct);

            product.Description = updateProduct.Description;
            product.Name = updateProduct.Name;
            product.Price = updateProduct.Price;
            product.Stock = updateProduct.Stock < 0 ? 0 : updateProduct.Stock;

            ////----


            var isPrevStockWasEmptyOrFullReserved = prevStock - product.ReservedStock == 0 || prevStock == 0;
            var isNewStockHaveFreeProds = product.Stock - product.ReservedStock > 0; //reserved stock cannot be changed by request
            if (isPrevStockWasEmptyOrFullReserved && isNewStockHaveFreeProds)
            {
                await dbcontext
                    .RequestNotifications.Include(q => q.User)
                    .Include(q => q.Product)
                    .Where(q => q.ProductID == product.ID)
                    .ForEachAsync(
                        async notification =>
                        {
                            await ProductEmailNotification.Notify(notification.User, notification.Product);
                            dbcontext.RequestNotifications.Remove(notification);
                        },
                        cancellationToken: cancellationToken
                    );
                await dbcontext.SaveChangesAsync(cancellationToken);
            }

            await ManyToManyEntitiesUpdater.UpateManyToManyRelationsAsync<Product, AttributeValue, ProductAttributeValue>(
                dbcontext,
                product,
                product.AttributeValues,
                updateProduct.AttributeValueIds,
                (prod, attrValue) =>
                    new ProductAttributeValue
                    {
                        Product = prod,
                        ProductID = prod.ID,
                        AttributeValue = attrValue,
                        AttributeValueID = attrValue.ID,
                    },
                async ids => await dbcontext.AttributeValues.Where(av => ids.Contains(av.ID)).ToListAsync(cancellationToken)
            );



            await dbcontext.SaveChangesAsync(cancellationToken);
        }
    }
}
