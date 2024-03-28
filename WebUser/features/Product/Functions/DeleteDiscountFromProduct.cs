using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.features.discount.DTO;
using WebUser.Domain.entities;
using WebUser.features.discount.Exceptions;
using WebUser.shared.RepoWrapper;
using WebUser.shared;

namespace WebUser.features.discount.Functions
{
    public class DeleteDiscountFromProduct
    {
        //input
        public class DeleteDiscountCommand : IRequest
        {
            public int ID { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<DeleteDiscountCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper ServiceWrapper)
            {
                _repoWrapper = ServiceWrapper;
            }

            public async Task Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
            {

                if (await _repoWrapper.Discount.IsExistsAsync(new ObjectID<E.Discount>(request.ID)))
                {
                    var discount = await _repoWrapper.Discount.GetByIdAsync(new ObjectID<E.Discount>(request.ID));
                    _repoWrapper.Discount.Delete(discount);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new DiscountNotFoundException(request.ID);
            }
        }

    }
}
