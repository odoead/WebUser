using AutoMapper;
using MediatR;
using Enteties = WebUser.Domain.entities;
using WebUser.features.OrderProduct.DTO;
using WebUser.Domain.entities;
using WebUser.features.OrderProduct.Exceptions;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.OrderProduct.Functions
{
    public class DeleteOrderProduct
    {
        //input
        public class DeleteCommand : IRequest
        {
            public int ID { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<DeleteCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper ServiceWrapper)
            {
                _repoWrapper = ServiceWrapper;
            }

            public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
            {

                if (await _repoWrapper.OrderProduct.IsExistsAsync(request.ID))
                {
                    var OrderProduct = await _repoWrapper.OrderProduct.GetByIdAsync(request.ID);
                    _repoWrapper.OrderProduct.Delete(OrderProduct);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new OrderProductNotFoundException(request.ID);
            }
        }

    }
}
