using AutoMapper;
using MediatR;
using WebUser.Domain.entities;
using WebUser.features.Order.DTO;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Order.Functions
{
    public class CreateOrder
    {
        //input
        public class CreateOrderCommand : IRequest<OrderDTO>
        {
            public User User { get; set; }
            public string DeliveryAddress { get; set; }
            public int DeliveryMethod { get; set; }
            public int PaymentMethod { get; set; }
            public bool Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public int CartId { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<CreateOrderCommand, OrderDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<OrderDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {


                var order = new E.Order
                {
                    CreatedAt = DateTime.Now,
                    DeliveryAddress = request.DeliveryAddress,
                    DeliveryMethod = request.DeliveryMethod,
                    PaymentMethod = request.PaymentMethod,
                    Status = request.Status,
                    User = request.User,

                };
                var Cart = await _repoWrapper.Cart.GetByUserIdAsync(new ObjectID<E.User>(request.User.Id));
                foreach (var item in Cart.items)
                {
                    var product = await _repoWrapper.Product.GetByIdAsync(new ObjectID<E.Product>(item.ProductId));
                    var amount = item.Amount >= item.Product.Stock ? item.Product.Stock : item.Amount;
                    product.Stock = product.Stock - amount;

                    order.OrderProduct.Add(
                        new E.OrderProduct
                        {
                            Amount = amount,
                            Product = item.Product,

                        });
                    Cart.items.Remove(item);
                }


                _repoWrapper.OrderProduct.CreateOrderProdFromCartItems(new ObjectID<E.Cart>(request.CartId), order);

                _repoWrapper.Order.Create(order);
                await _repoWrapper.SaveAsync();

                var CartItems = await _repoWrapper.CartItem.GetByCartIdAsync(new ObjectID<E.Cart>(request.CartId));
                _repoWrapper.OrderProduct.CreateOrderProdFromCartItems(new ObjectID<E.Cart>(request.CartId), order);

                /*order= await _repoWrapper.Order.GetByUserIdAsync(new shared.ObjectID<User>(request.User.Id));
                if()
                var results = _mapper.Map<OrderDTO>(order);
                return results;*/
            }
        }

    }
}
