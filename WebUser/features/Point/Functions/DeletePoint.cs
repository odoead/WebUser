using AutoMapper;
using MediatR;
using Enteties = WebUser.Domain.entities;
using WebUser.features.Point.DTO;
using WebUser.Domain.entities;
using WebUser.features.Point.Exceptions;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Point.Functions
{
    public class DeletePromotion
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

                if (await _repoWrapper.Point.IsExistsAsync(request.ID))
                {
                    var Point = await _repoWrapper.Point.GetByIdAsync(request.ID);
                    _repoWrapper.Point.Delete(Point);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new PointNotFoundException(request.ID);
            }
        }

    }
}
