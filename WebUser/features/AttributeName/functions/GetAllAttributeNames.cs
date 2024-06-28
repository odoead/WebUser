using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.Product.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.AttributeName.functions
{
    public class GetAllAttrNameAsync
    {
        //input
        public class GetAllAttrNameQuery : IRequest<ICollection<AttributeNameDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllAttrNameQuery, ICollection<AttributeNameDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<AttributeNameDTO>> Handle(GetAllAttrNameQuery request, CancellationToken cancellationToken)
            {
                var names = await dbcontext.AttributeNames.ToListAsync(cancellationToken: cancellationToken);
                var results = mapper.Map<ICollection<AttributeNameDTO>>(names);
                return PagedList<AttributeNameDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
