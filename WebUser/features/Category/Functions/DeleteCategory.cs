using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using WebUser.shared.RepoWrapper;
using WebUser.shared;

namespace WebUser.features.Category.Functions
{
    public class DeleteCategory
    {
        //input
        public class DeleteCategoryCommand : IRequest
        {
            public int ID { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<DeleteCategoryCommand>
        {
            private IRepoWrapper _repoWrapper;

            public Handler(IRepoWrapper ServiceWrapper)
            {
                _repoWrapper = ServiceWrapper;
            }

            public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {

                if (await _repoWrapper.Category.IsExistsAsync(new ObjectID<E.Category>(request.ID)))
                {
                    var category = await _repoWrapper.Category.GetByIdAsync(new ObjectID<E.Category>(request.ID));
                    _repoWrapper.Category.Delete(category);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new CategoryNotFoundException(request.ID);
            }
        }

    }
}
