using MediatR;
using WebUser.features.Promotion.Exceptions;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Promotion.Functions
{
    public class DeletePromotion
    {
        //input
        public class DeletePromotionCommand : IRequest
        {
            public int ID { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<DeletePromotionCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper ServiceWrapper)
            {
                _repoWrapper = ServiceWrapper;
            }

            public async Task Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
            {

                if (await _repoWrapper.Promotion.IsExistsAsync(request.ID))
                {
                    var Promotion = await _repoWrapper.Promotion.GetByIdAsync(request.ID);
                    _repoWrapper.Promotion.Delete(Promotion);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new PromotionNotFoundException(request.ID);

            }
        }

    }
}
