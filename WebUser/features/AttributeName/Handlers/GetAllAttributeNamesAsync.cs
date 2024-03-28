using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;

namespace WebUser.features.AttributeName.functions
{
    public class GetAllAttrNameAsync
    {
        //input
        public class GetAllAttrNameQuery : IRequest<ICollection<AttributeNameDTO>> { }
        //handler
        public class Handler : IRequestHandler<GetAllAttrNameQuery, ICollection<AttributeNameDTO>>
        {
            private DB_Context dbcontext;
            private IMapper _mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                _mapper = mapper;
            }

            public async Task<ICollection<AttributeNameDTO>> Handle(GetAllAttrNameQuery request, CancellationToken cancellationToken)
            {
                var names = await dbcontext.attributeNames.ToListAsync();
                var results = _mapper.Map<ICollection<AttributeNameDTO>>(names);
                return results;
            }
        }

    }
}
