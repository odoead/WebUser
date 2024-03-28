using AutoMapper;
using MediatR;
using Enteties = WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using E=WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebUser.features.AttributeName.DTO;
using WebUser.shared.RepoWrapper;
using WebUser.features.AttributeName.Exceptions;
using WebUser.shared;

namespace WebUser.features.AttributeName.functions
{
    public class UpdateAttributeName
    {
        //input
        public class UpdateAttrNameCommand : IRequest
        {
            public int ID { get; set; }
            public AttributeNameUpdateDTO attributeName { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<UpdateAttrNameCommand>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper repoWrapper, IMapper mapper)
            {

                _repoWrapper = repoWrapper;
                _mapper = mapper;
            }

            public async Task Handle(UpdateAttrNameCommand request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.AttributeName.IsExistsAsync(new ObjectID<E.AttributeName>(request.ID)))
                {
                    var name = await _repoWrapper.AttributeName.GetByIdAsync(new ObjectID<E.AttributeName>(request.ID));
                    _mapper.Map(request.attributeName, name);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new AttributeNameNotFoundException(request.ID);
            }
        }

    }
}
