using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.Exceptions;

namespace WebUser.features.Cart.functions
{
    public class DeleteCart
    {
        //input
        public class DeleteCartCommand : IRequest
        {
            public int ID { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<DeleteCartCommand>
        {
            private readonly DB_Context _dbcontext;

            public Handler(DB_Context dbcontex)
            {
                _dbcontext = dbcontex;
            }

            public async Task Handle(DeleteCartCommand request, CancellationToken cancellationToken)
            {
                var cart =
                    await _dbcontext.Carts.Where(q => q.ID == request.ID).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new CartNotFoundException(request.ID);
                _dbcontext.Carts.Remove(cart);
                await _dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
