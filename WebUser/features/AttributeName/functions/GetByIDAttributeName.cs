using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.AttributeValue.DTO;

namespace WebUser.features.AttributeName.functions
{
    public class GetByIDAttributeName
    {
        //input
        public class GetByIDAttrNameQuery : IRequest<AttributeNameDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetByIDAttrNameQuery, AttributeNameDTO>
        {
            private readonly DB_Context dbcontext;


            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<AttributeNameDTO> Handle(GetByIDAttrNameQuery request, CancellationToken cancellationToken)
            {
                var name =
                    await dbcontext
                        .AttributeNames.Include(q => q.AttributeValues)
                        .Where(q => q.ID == request.Id)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new AttributeNameNotFoundException(request.Id);

                var results = new AttributeNameDTO
                {
                    AttributeValues = new List<AttributeValueDTO>(),
                    Name = name.Name,
                    Description = name.Description,
                    Id = name.ID,
                };
                foreach (var attributeValue in results.AttributeValues)
                {
                    results.AttributeValues.Add(new AttributeValueDTO { ID = attributeValue.ID, Value = attributeValue.Value });
                }
                return results;
            }
        }
    }
}
