using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeName.Exceptions;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using WebUser.Data;
using Microsoft.EntityFrameworkCore;

namespace WebUser.features.AttributeName.functions
{
    public class GetByIDAttributeNameAsync
    {
        //input
        public class GetByIDAttrNameQuery : IRequest<AttributeNameDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetByIDAttrNameQuery, AttributeNameDTO>
        {
            private DB_Context dbcontext;
            private IMapper _mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                _mapper = mapper;
            }

            public async Task<AttributeNameDTO> Handle(GetByIDAttrNameQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.attributeNames.AnyAsync(q=>q.ID==request.Id))
                {
                    var name = await dbcontext.attributeNames.Where(q=>q.ID==request.Id).FirstOrDefaultAsync();
                    var results = _mapper.Map<AttributeNameDTO>(name);
                    return results;
                }else
                throw new AttributeNameNotFoundException(request.Id);
            }
        }
    }
}
