using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.AttributeValue.Exceptions;

namespace WebUser.features.AttributeName.functions
{
    public class DeleteAttributeValueFromAttrName
    {
        public class DeleteAttributeValueCommand : IRequest
        {
            public int AttributeNameId { get; set; }
            public int AttributeValueId { get; set; }
        }
        public class Handler : IRequestHandler<DeleteAttributeValueCommand>
        {
            private DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteAttributeValueCommand request, CancellationToken cancellationToken)
            {
                if (await dbcontext.attributeNames.AnyAsync(q => q.ID == request.AttributeNameId))
                    throw new AttributeNameNotFoundException(request.AttributeNameId);
                if (await dbcontext.attributeValues.AnyAsync(q => q.ID == request.AttributeValueId))
                    throw new AttributeValueNotFoundException(request.AttributeValueId);
                var AttributeName = await dbcontext.attributeNames.Include(q => q.AttributeValues).SingleOrDefaultAsync(q => q.ID == request.AttributeNameId);
                var attributeValue = await dbcontext.attributeValues.SingleOrDefaultAsync(q => q.ID == request.AttributeValueId);
                if (AttributeName.AttributeValues.Contains(attributeValue))
                {
                    AttributeName.AttributeValues.Remove(attributeValue);
                    await dbcontext.SaveChangesAsync();
                }

            }
        }

    }
}
