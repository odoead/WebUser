using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.Exceptions;
using E = WebUser.Domain.entities;

namespace WebUser.features.AttributeName.functions
{
    public class AddAttributeValueToAttrName
    {
        public class AddAttributeValueCommand : IRequest
        {
            [Required]
            public string AttributeValue { get; set; }
            [Required]
            public int AttributeNameID { get; set; }
        }

        public class Handler : IRequestHandler<AddAttributeValueCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(AddAttributeValueCommand request, CancellationToken cancellationToken)
            {
                var attributeName =
                    await dbcontext
                        .AttributeNames.Include(a => a.AttributeValues)
                        .FirstOrDefaultAsync(q => q.ID == request.AttributeNameID, cancellationToken)
                    ?? throw new AttributeNameNotFoundException(request.AttributeNameID);

                if (!attributeName.AttributeValues.Any(av => av.Value == request.AttributeValue))
                {
                    attributeName.AttributeValues.Add(
                        new E.AttributeValue
                        {
                            AttributeName = attributeName,
                            Value = request.AttributeValue,
                            AttributeNameID = request.AttributeNameID,
                        }
                    );

                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
