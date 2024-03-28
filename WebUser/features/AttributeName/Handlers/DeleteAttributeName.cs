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
            private DB_Context dbcontext;


            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task Handle(DeleteAttributeNameCommand request, CancellationToken cancellationToken)
            {

                if (await dbcontext.attributeNames.AnyAsync(q => q.ID == request.ID))
                {
                    var name = await dbcontext.attributeNames.Where(q => q.ID == request.ID).FirstOrDefaultAsync();
                    dbcontext.attributeNames.Remove(name);
                    await dbcontext.SaveChangesAsync();

                }
                else
                    throw new AttributeNameNotFoundException(request.ID);
            }
        }

    }
}
