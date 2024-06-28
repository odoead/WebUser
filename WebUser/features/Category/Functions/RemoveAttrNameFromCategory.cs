using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.Category.Exceptions;

namespace WebUser.features.Category.Functions
{
    public class RemoveAttrNameFromCategory
    {
        public class RemoveAttrNameFromCategoryCommand : IRequest
        {
            public int CategoryId { get; set; }
            public int AttributeNameID { get; set; }
        }

        public class Handler : IRequestHandler<RemoveAttrNameFromCategoryCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(RemoveAttrNameFromCategoryCommand request, CancellationToken cancellationToken)
            {
                var category =
                    await dbcontext.Categories.Where(q => q.ID == request.CategoryId).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new CategoryNotFoundException(request.CategoryId);
                var attrName =
                    await dbcontext
                        .AttributeNames.Where(q => q.ID == request.AttributeNameID)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new AttributeNameNotFoundException(request.AttributeNameID);
                if (category.Attributes.Select(q => q.AttributeName).Contains(attrName))
                {
                    category.Attributes.Remove(new AttributeNameCategory { AttributeName = attrName, Category = category });
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
