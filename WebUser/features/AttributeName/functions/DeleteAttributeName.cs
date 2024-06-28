using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.Exceptions;

namespace WebUser.features.AttributeName.functions
{
    public class DeleteAttributeName
    {
        //input
        public class DeleteAttributeNameCommand : IRequest
        {
            public int ID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<DeleteAttributeNameCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteAttributeNameCommand request, CancellationToken cancellationToken)
            {
                var name =
                    await dbcontext.AttributeNames.Where(q => q.ID == request.ID).FirstOrDefaultAsync(cancellationToken)
                    ?? throw new AttributeNameNotFoundException(request.ID);
                dbcontext.AttributeNames.Remove(name);
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
