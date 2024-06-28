using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Category.Exceptions;

namespace WebUser.features.Category.Functions
{
    public class DeleteCategory
    {
        //input
        public class DeleteCategoryCommand : IRequest
        {
            public int ID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<DeleteCategoryCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context dbcontex)
            {
                dbcontext = dbcontex;
            }

            public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                var category =
                    await dbcontext.Categories.Where(q => q.ID == request.ID).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new CategoryNotFoundException(request.ID);
                dbcontext.Categories.Remove(category);
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
