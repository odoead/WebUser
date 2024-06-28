using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Point.Exceptions;

namespace WebUser.features.Point.Functions
{
    public class DeletePoint
    {
        //input
        public class DeletePointCommand : IRequest
        {
            public int ID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<DeletePointCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeletePointCommand request, CancellationToken cancellationToken)
            {
                var item =
                    await dbcontext.Points.Where(q => q.ID == request.ID).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new PointNotFoundException(request.ID);
                dbcontext.Points.Remove(item);
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
