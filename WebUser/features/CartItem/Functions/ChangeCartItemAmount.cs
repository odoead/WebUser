using MediatR;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.CartItem.Functions
{
    public class ChangeCartItemAmount
    {
        //intput
        public class ChangeCartItemAmountCommand : IRequest
        {
            public int CartItemId { get; set; }
            public int NewAmount { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<ChangeCartItemAmountCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper repoWrapper)
            {
                _repoWrapper = repoWrapper;
            }

            public async Task Handle(ChangeCartItemAmountCommand request, CancellationToken cancellationToken)
            {
                E.CartItem cartItem = await _repoWrapper.CartItem.GetByIdAsync(new ObjectID<E.CartItem>(request.CartItemId));
                var product = await _repoWrapper.Product.GetByIdAsync(new ObjectID<E.Product>(cartItem.ProductId));
                if (product.Stock <= request.NewAmount)
                {
                    cartItem.Amount = product.Stock;
                }
                else
                    cartItem.Amount = request.NewAmount;
                await _repoWrapper.SaveAsync();

            }
        }
    }
}
