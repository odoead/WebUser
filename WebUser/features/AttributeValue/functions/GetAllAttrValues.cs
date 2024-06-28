using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Product.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.AttributeValue.functions
{
    public class GetAllAttrValues
    {
        //input
        public class GetAllAttrValueQuery : IRequest<ICollection<AttributeValueDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllAttrValueQuery, ICollection<AttributeValueDTO>>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<AttributeValueDTO>> Handle(GetAllAttrValueQuery request, CancellationToken cancellationToken)
            {
                var values = await dbcontext.AttributeValues.ToListAsync(cancellationToken: cancellationToken);
                var results = mapper.Map<ICollection<AttributeValueDTO>>(values);
                return PagedList<AttributeValueDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
