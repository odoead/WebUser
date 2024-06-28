using AutoMapper;
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
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<AttributeNameDTO> Handle(CreateAttributeNameCommand request, CancellationToken cancellationToken)
            {
                var attributeName = new E.AttributeName { Name = request.Name, Description = request.Description, };
                if (
                    !await dbcontext.AttributeNames.AnyAsync(
                        q => q.Name == request.Name && q.Description == request.Description,
                        cancellationToken: cancellationToken
                    )
                )
                {
                    await dbcontext.AttributeNames.AddAsync(attributeName, cancellationToken);
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
                var results = mapper.Map<AttributeNameDTO>(attributeName);
                return results;
            }
        }
    }
}
