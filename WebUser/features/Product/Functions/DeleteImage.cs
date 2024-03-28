using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.features.discount.DTO;
using WebUser.Domain.entities;
using WebUser.shared.RepoWrapper;
using WebUser.features.Image.Exceptions;
using WebUser.shared;
namespace WebUser.features.Product.Functions
{
    public class DeleteImage
    {
        //input
        public class DeleteImageCommand : IRequest
        {
            public int ID { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<DeleteImageCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper ServiceWrapper)
            {
                _repoWrapper = ServiceWrapper;
            }

            public async Task Handle(DeleteImageCommand request, CancellationToken cancellationToken)
            {

                if (await _repoWrapper.Image.IsExistsAsync(new ObjectID<E.Image>(request.ID)))
                {
                    var img = await _repoWrapper.Image.GetByIdAsync(new ObjectID<E.Image>(request.ID));
                    _repoWrapper.Image.Delete(img);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new ImageNotFoundException(request.ID);
            }
        }

    }
}
