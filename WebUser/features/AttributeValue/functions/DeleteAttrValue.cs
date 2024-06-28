using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeValue.Exceptions;

namespace WebUser.features.AttributeValue.functions
{
    public class DeleteAttrValue
    {
        //input
        public class DeleteAttributeValueCommand : IRequest
        {
            public int ID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<DeleteAttributeValueCommand>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteAttributeValueCommand request, CancellationToken cancellationToken)
            {
                var value =
                    await dbcontext.AttributeValues.Where(q => q.ID == request.ID).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new AttributeValueNotFoundException(request.ID);
                dbcontext.AttributeValues.Remove(value);
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
