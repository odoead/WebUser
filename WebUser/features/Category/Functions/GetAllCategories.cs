using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.Category.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Category.Functions
{
    public class GetAllCategories
    {
        //input
        public class GetAllCategoryAsyncQuery : IRequest<ICollection<CategoryDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllCategoryAsyncQuery, ICollection<CategoryDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<CategoryDTO>> Handle(GetAllCategoryAsyncQuery request, CancellationToken cancellationToken)
            {
                var categories = await dbcontext.Categories.ToListAsync(cancellationToken: cancellationToken);
                var results = mapper.Map<ICollection<CategoryDTO>>(categories);
                return PagedList<CategoryDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
