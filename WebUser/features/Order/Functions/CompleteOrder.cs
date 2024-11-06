namespace WebUser.features.Order.Functions;

using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Order.Exceptions;

public class CompleteOrder
{
    public class CompleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<CompleteOrderCommand>
    {
        private readonly DB_Context _dbcontext;

        public Handler(DB_Context context)
        {
            _dbcontext = context;
        }

        public async Task Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order =
                await _dbcontext
                    .Orders.Include(o => o.OrderProducts)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.ID == request.Id, cancellationToken) ?? throw new OrderNotFoundException(request.Id);

            //check f order havent been completed yet
            if (order.IsCompleted == true)
            {
                throw new InvalidOperationException($"Order {request.Id} is already completed.");
            }

            foreach (var orderItem in order.OrderProducts)
            {
                var product = orderItem.Product;

                if (product.ReservedStock < orderItem.Amount || product.Stock < orderItem.Amount)
                {
                    throw new ProductStockException(product.ID);
                }
                product.ReservedStock -= orderItem.Amount;
                product.Stock -= orderItem.Amount;

                _dbcontext.Products.Update(product);
            }

            //set as completed
            order.IsCompleted = true;

            _dbcontext.Orders.Update(order);
            await _dbcontext.SaveChangesAsync(cancellationToken);
        }
    }
}
