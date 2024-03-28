using AutoMapper;
using MediatR;
using Enteties = WebUser.Domain.entities;
using WebUser.features.Order.DTO;
using WebUser.Domain.entities;
using WebUser.features.Order.Exceptions;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Order.Functions
{
    public class DeleteOrder
    {
        //input
        public class DeleteOrderCommand : IRequest
        {
            public int ID { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<DeleteOrderCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper ServiceWrapper)
            {
                _repoWrapper = ServiceWrapper;
            }

            public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {

                if (await _repoWrapper.Order.IsExistsAsync(request.ID))
                {
                    var Order = await _repoWrapper.Order.GetByIdAsync(request.ID);
                    _repoWrapper.Order.Delete(Order);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new OrderNotFoundException(request.ID);
            }
        }

    }
}
