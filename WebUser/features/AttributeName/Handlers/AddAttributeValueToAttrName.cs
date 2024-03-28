using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.AttributeValue.Exceptions;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.AttributeName.functions
{
    public class AddAttributeValueToAttrName
    {
        public class AddAttributeValueCommand : IRequest
        {
            public int AttributeValueID { get; set; }
            public int AttributeNameID { get; set; }
        }

        public class Handler : IRequestHandler<AddAttributeValueCommand>
        {
            private IRepoWrapper _repoWrapper;
            private DB_Context dbcontext;

            public Handler(IRepoWrapper repoWrapper, DB_Context context)
            {
                dbcontext = context;
                _repoWrapper = repoWrapper;
            }
            public async Task Handle(AddAttributeValueCommand request, CancellationToken cancellationToken)
            {
                if (await dbcontext.attributeNames.AnyAsync(q => q.ID == request.AttributeNameID))
                    throw new AttributeNameNotFoundException(request.AttributeNameID);
                if (await dbcontext.attributeValues.AnyAsync(q => q.ID == request.AttributeValueID))
                    throw new AttributeValueNotFoundException(request.AttributeValueID);
                var AttributeName = await dbcontext.attributeNames.Where(q => q.ID == request.AttributeNameID).FirstOrDefaultAsync();
                var attributeValue = await dbcontext.attributeValues.Where(q => q.ID == request.AttributeValueID).FirstOrDefaultAsync();
                if (!AttributeName.AttributeValues.Contains(attributeValue))
                {
                    AttributeName.AttributeValues.Add(attributeValue);
                    await dbcontext.SaveChangesAsync();
                }


            }
        }
    }
}
