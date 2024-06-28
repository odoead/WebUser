using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.DTO;
using WebUser.features.Category.Exceptions;

namespace WebUser.features.Category.Functions
{
    public class GetCategoryByID
    {
        //input
        public class GetCategoryByIDQuery : IRequest<CategoryDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetCategoryByIDQuery, CategoryDTO>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<CategoryDTO> Handle(GetCategoryByIDQuery request, CancellationToken cancellationToken)
            {
                var categories =
                    await dbcontext.Categories.FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken)
                    ?? throw new CategoryNotFoundException(request.Id);
                var results = mapper.Map<CategoryDTO>(categories);
                return results;
            }
        }
    }
}
