/*using AutoMapper;
using MediatR;
using Enteties = WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using E = WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using WebUser.shared.RepoWrapper;
using WebUser.features.CartItem.Exceptions;
using WebUser.shared;

namespace WebUser.features.CartItem.functions
{
    public class DeleteCartItem
    {
        //input
        public class DeleteCartItemCommand : IRequest
        {
            public int ID { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<DeleteCartItemCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper repoWrapper)
            {
                _repoWrapper = repoWrapper;
            }

            public async Task Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
            {

                if (await _repoWrapper.CartItem.IsExistsAsync(new ObjectID<Enteties.CartItem>(request.ID)))
                {
                    var name = await _repoWrapper.CartItem.GetByIdAsync(new ObjectID<Enteties.CartItem>(request.ID));
                    _repoWrapper.CartItem.Delete(name);
                    await _repoWrapper.SaveAsync();
                }
                else
                    throw new CartItemNotFoundException(request.ID);
            }
        }

    }
}
*/