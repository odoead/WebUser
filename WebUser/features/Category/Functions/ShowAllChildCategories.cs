using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.DTO;
using WebUser.features.Category.Exceptions;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Category.Functions
{
    public class ShowAllChildCategories
    {
        //input
        public class GetAllChildCategoriesQuery : IRequest<ICollection<CartDTO>>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllChildCategoriesQuery, ICollection<CartDTO>>
        {
            private readonly IMapper mapper;
            private readonly DB_Context dbcontext;
            private readonly IServiceWrapper repo;

            public Handler(DB_Context context, IMapper mapper, IServiceWrapper repository)
            {
                repo = repository;
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<CartDTO>> Handle(GetAllChildCategoriesQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Categories.AnyAsync(q => q.ID == request.Id, cancellationToken: cancellationToken))
                    throw new CategoryNotFoundException(request.Id);
                var children = await repo.Category.ShowAllChildCategories(request.Id) ?? throw new CategoryNotFoundException(-1);
                ;
                var results = mapper.Map<ICollection<CartDTO>>(children);
                return results;
            }
        }
    }
}
