namespace WebUser.features.AttributeValue.functions;

using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeValue.Exceptions;

public class UpdateAttributeValue
{
    //input
    public class UpdateAttributeValueCommand : IRequest
    {
        public UpdateAttributeValueCommand(int id, string updateValue)
        {
            this.Id = id;
            this.UpdateValue = updateValue;
        }

        public int Id { get; set; }
        public string UpdateValue { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<UpdateAttributeValueCommand>
    {
        private readonly DB_Context dbcontext;


        public Handler(DB_Context context)
        {
            dbcontext = context;

        }

        public async Task Handle(UpdateAttributeValueCommand request, CancellationToken cancellationToken)
        {
            var attributeValue =
                await dbcontext.AttributeValues.Where(q => q.ID == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? throw new AttributeValueNotFoundException(request.Id);
            attributeValue.Value = request.UpdateValue;
            await dbcontext.SaveChangesAsync(cancellationToken);
        }
    }
}
