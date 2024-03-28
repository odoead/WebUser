using AutoMapper;
using MediatR;
using WebUser.features.Cart.Exceptions;
using WebUser.features.Product.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart.functions
{
    public class AddProductsToCart
    {
        public class AddProductToCartCommand : IRequest
        {
            public int ProductId { get; set; }
            public int CartId { get; set; }
            public int Amount { get; set; }
        }
        public class Handler : IRequestHandler<AddProductToCartCommand>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper repoWrapper, IMapper mapper)
            {
                _repoWrapper = repoWrapper;
                _mapper = mapper;
            }
            public async Task Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
            {

                if (!await _repoWrapper.Product.IsExistsAsync(new ObjectID<E.Product>(request.ProductId)))
                    throw new ProductNotFoundException(request.ProductId);
                if (!await _repoWrapper.Cart.IsExistsAsync(new ObjectID<E.Cart>(request.CartId)))
                    throw new CartNotFoundException(request.CartId);
                var Cart = await _repoWrapper.Cart.GetByIdAsync(new ObjectID<E.Cart>(request.CartId));
                var products = await _repoWrapper.Product.GetByIdAsync(new ObjectID<E.Product>(request.ProductId));
                _repoWrapper.Cart.AddProduct(products, request.Amount, Cart);

                //_mapper.Map(AttributeName, await _repoWrapper.AttributeName.GetByIdAsync(new ObjectID<E.AttributeName>(request.AttributeNameID)));
                await _repoWrapper.SaveAsync();

            }
        }
    }
}
