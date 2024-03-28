using AutoMapper;
using MediatR;
using Enteties = WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebUser.features.AttributeValue.DTO;
using WebUser.shared.RepoWrapper;
using WebUser.shared;
using E = WebUser.Domain.entities;


namespace WebUser.features.Category.Functions
{
    public class UpdateCategory
    {
        //input
        public class UpdateCategoryCommand : IRequest
        {
            public int ID { get; set; }
            public AttributeValueUpdateDTO Category { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<UpdateCategoryCommand>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {

                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Category.IsExistsAsync(new ObjectID<E.Category>(request.ID)))
                {
                    var category = await _repoWrapper.Category.GetByIdAsync(new ObjectID<E.Category>(request.ID));
                    _mapper.Map(request.Category, category);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new CategoryNotFoundException(request.ID);
            }
        }

    }
}
