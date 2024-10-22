using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.AttributeValue.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.AttributeName.functions
{
    public class GetAllAttrNameAsync
    {
        //input
        public class GetAllAttrNameQuery : IRequest<PagedList<AttributeNameDTO>>
        {
            public AttributeNameRequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllAttrNameQuery, PagedList<AttributeNameDTO>>
        {
            private readonly DB_Context dbcontext;


            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<PagedList<AttributeNameDTO>> Handle(GetAllAttrNameQuery request, CancellationToken cancellationToken)
            {
                var data = dbcontext.AttributeNames.Include(q => q.AttributeValues).AsQueryable();
                var src = await data.Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                    .Take(request.Parameters.PageSize)
                    .ToListAsync(cancellationToken);
                var dto = new List<AttributeNameDTO>();
                foreach (var attributeName in src)
                {
                    var attributeNameDTO = new AttributeNameDTO
                    {
                        Id = attributeName.ID,
                        Name = attributeName.Name,
                        Description = attributeName.Description,
                        AttributeValues = new List<AttributeValueDTO>(),
                    };

                    foreach (var attributeValue in attributeName.AttributeValues)
                    {
                        attributeNameDTO.AttributeValues.Add(new AttributeValueDTO { ID = attributeValue.ID, Value = attributeValue.Value });
                    }

                    dto.Add(attributeNameDTO);
                }
                var pagedList = PagedList<AttributeNameDTO>.PaginateList(
                    source: dto,
                    totalCount: await data.CountAsync(cancellationToken: cancellationToken),
                    pageNumber: request.Parameters.PageNumber,
                    pageSize: request.Parameters.PageSize
                );
                return pagedList;
            }
        }
    }
}
