using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.AttributeValue.Exceptions;

namespace WebUser.features.AttributeValue.functions
{
    public class GetByIDAttributeValue
    {
        //input
        public class GetByIDAttrValueQuery : IRequest<AttributeValueDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetByIDAttrValueQuery, AttributeValueDTO>
        {

            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<AttributeValueDTO> Handle(GetByIDAttrValueQuery request, CancellationToken cancellationToken)
            {
                var value =
                    await dbcontext.AttributeValues.FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken)
                    ?? throw new AttributeValueNotFoundException(request.Id);
                var results = new AttributeValueDTO { ID = value.ID, Value = value.Value };

                return results;
            }
        }
    }
}
