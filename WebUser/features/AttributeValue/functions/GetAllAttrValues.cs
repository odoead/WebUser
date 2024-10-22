using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeValue.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.AttributeValue.functions
{
    public class GetAllAttrValues
    {
        //input
        public class GetAllAttrValueQuery : IRequest<PagedList<AttributeValueDTO>>
        {
            public GetAllAttrValueQuery(AttributeValuesRequestParameters Parameters)
            {
                this.Parameters = Parameters;
            }

            public AttributeValuesRequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllAttrValueQuery, PagedList<AttributeValueDTO>>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<PagedList<AttributeValueDTO>> Handle(GetAllAttrValueQuery request, CancellationToken cancellationToken)
            {
                var dataContext = dbcontext.AttributeValues.AsQueryable();
                var src = await dataContext
                    .Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                    .Take(request.Parameters.PageSize)
                    .ToListAsync(cancellationToken);
                var dto = new List<AttributeValueDTO>();
                foreach (var attrb in src)
                {
                    dto.Add(new AttributeValueDTO { ID = attrb.ID, Value = attrb.Value });
                }

                var pagedList = PagedList<AttributeValueDTO>.PaginateList(
                    source: dto,
                    totalCount: await dataContext.CountAsync(cancellationToken: cancellationToken),
                    pageNumber: request.Parameters.PageNumber,
                    pageSize: request.Parameters.PageSize
                );
                /*foreach (var item in pagedList)
                {
                    Console.WriteLine(item);
                }*/
                return pagedList;
            }
        }
    }
}
