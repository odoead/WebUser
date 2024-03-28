using AutoMapper;
using MediatR;
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
            public int Id { get; set; }
            public string Name { get; set; }
            public ICollection<E.AttributeValue> Attributes { get; set; }
            public string Description { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreateAttributeNameCommand, AttributeNameDTO>
        {
            private IMapper _mapper;
            private DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                _mapper = mapper;

            }

            public async Task<AttributeNameDTO> Handle(CreateAttributeNameCommand request, CancellationToken cancellationToken)
            {
                var attributeName = new E.AttributeName
                {
                    Name = request.Name,
                    Description = request.Description,
                    AttributeValues = request.Attributes
                };
                await dbcontext.attributeNames.AddAsync(attributeName);
                await dbcontext.SaveChangesAsync();
                var results = _mapper.Map<AttributeNameDTO>(attributeName);
                return results;
            }
        }

    }
}
