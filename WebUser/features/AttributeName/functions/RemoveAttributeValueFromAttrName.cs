using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.Exceptions;

namespace WebUser.features.AttributeName.functions
{
    public class RemoveAttributeValueFromAttrName
    {
        public class DeleteAttributeValueCommand : IRequest
        {
            public int AttributeNameId { get; set; }
            public int AttributeValueId { get; set; }
        }

        public class Handler : IRequestHandler<DeleteAttributeValueCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteAttributeValueCommand request, CancellationToken cancellationToken)
            {
                var attributeName =
                    await dbcontext
                        .AttributeNames.Include(q => q.AttributeValues)
                        .FirstOrDefaultAsync(q => q.ID == request.AttributeNameId, cancellationToken: cancellationToken)
                    ?? throw new AttributeNameNotFoundException(request.AttributeNameId);

                var attributeValue = await dbcontext
                    .AttributeValues.Where(q => q.AttributeNameID == request.AttributeNameId && q.ID == request.AttributeValueId)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                if (attributeValue != null)
                {
                    dbcontext.AttributeValues.Remove(attributeValue);
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
