using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.AttributeName.functions
{
    public class CreateAttributeName
    {
        //input
        public class CreateAttributeNameCommand : IRequest<AttributeNameDTO>
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateAttributeNameCommand, AttributeNameDTO>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<AttributeNameDTO> Handle(CreateAttributeNameCommand request, CancellationToken cancellationToken)
            {
                var existingAttribute = await dbcontext.AttributeNames.FirstOrDefaultAsync(
                    q => q.Name == request.Name && q.Description == request.Description,
                    cancellationToken
                );

                if (existingAttribute != null)
                {
                    return new AttributeNameDTO
                    {
                        Description = existingAttribute.Description,
                        Id = existingAttribute.ID,
                        Name = existingAttribute.Name,
                        AttributeValues = new(),
                    };
                }

                var attributeName = new E.AttributeName { Name = request.Name, Description = request.Description };

                await dbcontext.AttributeNames.AddAsync(attributeName, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);
                return new AttributeNameDTO
                {
                    Description = attributeName.Description,
                    Id = attributeName.ID,
                    Name = attributeName.Name,
                    AttributeValues = new(),
                };
            }
        }
    }
}
