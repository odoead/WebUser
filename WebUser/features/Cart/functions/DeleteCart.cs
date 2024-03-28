using MediatR;
using WebUser.features.Cart.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart.functions
{
    public class DeleteCart
    {
        //input
        public class DeleteAttributeNameCommand : IRequest
        {
            public int ID { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<DeleteAttributeNameCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper repoWrapper)
            {
                _repoWrapper = repoWrapper;
            }

            public async Task Handle(DeleteAttributeNameCommand request, CancellationToken cancellationToken)
            {

                if (await _repoWrapper.Cart.IsExistsAsync(new ObjectID<E.Cart>(request.ID)))
                {
                    var name = await _repoWrapper.Cart.GetByIdAsync(new ObjectID<E.Cart>(request.ID));
                    _repoWrapper.Cart.Delete(name);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new CartNotFoundException(request.ID);
            }
        }

    }
}
