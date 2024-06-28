using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.Exceptions;

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
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<AttributeNameDTO> Handle(GetByIDAttrNameQuery request, CancellationToken cancellationToken)
            {
                var name =
                    await dbcontext.AttributeNames.Where(q => q.ID == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new AttributeNameNotFoundException(request.Id);
                var results = mapper.Map<AttributeNameDTO>(name);
                return results;
            }
        }
    }
}
