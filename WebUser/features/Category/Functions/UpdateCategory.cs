namespace WebUser.features.Category.Functions;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;

public class UpdateCategory
{
    //input
    public class UpdateCategoryCommand : IRequest
    {
        public int Id { get; set; }
        public JsonPatchDocument<UpdateCategoryDTO> PatchDoc { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly DB_Context dbcontext;

        public Handler(DB_Context context)
        {
            dbcontext = context;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.PatchDoc == null)
            {
                return;
            }
            var category =
                await dbcontext
                    .Categories.Include(c => c.Attributes)
                    .Include(c => c.ParentCategory)
                    .Include(c => c.Subcategories)
                    .FirstOrDefaultAsync(c => c.ID == request.Id, cancellationToken: cancellationToken)
                ?? throw new CategoryNotFoundException(request.Id);

            var categoryToUpdate = new UpdateCategoryDTO
            {
                Name = category.Name,
                AttributeNameIds = category.Attributes.Select(q => q.AttributeNameID).ToList(),
                SubcategoryIds = category.Subcategories.Select(q => q.ID).ToList(),
                ParentCategoryId = category.ParentCategoryID ?? 0,
            };

            request.PatchDoc.ApplyTo(categoryToUpdate);

            category.Name = categoryToUpdate.Name;
            if (categoryToUpdate.AttributeNameIds != null)
            {
                category.Attributes.Clear();
                foreach (var attrId in categoryToUpdate.AttributeNameIds)
                {
                    var attributeName = await dbcontext.AttributeNames.FirstOrDefaultAsync(q => q.ID == attrId, cancellationToken);
                    if (attributeName != null)
                    {
                        category.Attributes.Add(new AttributeNameCategory { AttributeNameID = attributeName.ID, CategoryID = category.ID });
                    }
                }
            }

            if (categoryToUpdate.ParentCategoryId != 0)
            {
                var parentCategory = await dbcontext.Categories.FirstOrDefaultAsync(
                    q => q.ID == categoryToUpdate.ParentCategoryId,
                    cancellationToken
                );
                category.ParentCategory = parentCategory;
                category.ParentCategoryID = parentCategory?.ID;
            }
            else
            {
                category.ParentCategory = null;
                category.ParentCategoryID = null;
            }

            if (categoryToUpdate.SubcategoryIds != null)
            {
                category.Subcategories.Clear();

                var subCategories = await dbcontext
                    .Categories.Where(q => categoryToUpdate.SubcategoryIds.Contains(q.ID))
                    .ToListAsync(cancellationToken);

                foreach (var subCategory in subCategories)
                {
                    category.Subcategories.Add(subCategory);
                }
            }

            await dbcontext.SaveChangesAsync(cancellationToken);
        }
    }
}
