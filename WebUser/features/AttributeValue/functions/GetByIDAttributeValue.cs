using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Category.Exceptions;

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
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<AttributeValueDTO> Handle(GetByIDAttrValueQuery request, CancellationToken cancellationToken)
            {
                var name =
                    await dbcontext.AttributeValues.FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken)
                    ?? throw new CategoryNotFoundException(request.Id);
                var results = mapper.Map<AttributeValueDTO>(name);
                return results;
            }
        }
    }
}
