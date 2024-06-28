using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeValue.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.AttributeValue.functions
{
    public class CreateAttrValue
    {
        //input
        public class CreateAttributeValueCommand : IRequest<AttributeValueDTO>
        {
            public string Value { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreateAttributeValueCommand, AttributeValueDTO>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<AttributeValueDTO> Handle(CreateAttributeValueCommand request, CancellationToken cancellationToken)
            {
                var attributeValue = new E.AttributeValue { Value = request.Value, };
                if (!await dbcontext.AttributeValues.AnyAsync(q => q.Value == request.Value, cancellationToken))
                {
                    await dbcontext.AttributeValues.AddAsync(attributeValue, cancellationToken);
                    await dbcontext.SaveChangesAsync(cancellationToken);
                }
                var results = mapper.Map<AttributeValueDTO>(attributeValue);
                return results;
            }
        }
    }
}
