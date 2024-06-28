using AutoMapper;
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
            public string AttributeValue { get; set; }
            public int AttributeNameID { get; set; }
        }

        public class Handler : IRequestHandler<AddAttributeValueCommand>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(IMapper mapper, DB_Context context)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task Handle(AddAttributeValueCommand request, CancellationToken cancellationToken)
            {
                var attributeName =
                    await dbcontext
                        .AttributeNames.Where(q => q.ID == request.AttributeNameID)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new AttributeNameNotFoundException(request.AttributeNameID);
                var attributeValue =
                    await dbcontext
                        .AttributeValues.Where(q => q.AttributeName == attributeName && q.Value == request.AttributeValue)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? new E.AttributeValue { AttributeName = attributeName, Value = request.AttributeValue };
                attributeName.AttributeValues.Add(attributeValue);
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
