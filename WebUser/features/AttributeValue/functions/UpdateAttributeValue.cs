using AutoMapper;
using MediatR;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.AttributeValue.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.AttributeValue.functions
{
    public class UpdateAttributeValue
    {
        //input
        public class UpdateAttrValueCommand : IRequest
        {
            public int ID { get; set; }
            public AttributeValueUpdateDTO attributeValue { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<UpdateAttrValueCommand>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper repoWrapper, IMapper mapper)
            {

                _repoWrapper = repoWrapper;
                _mapper = mapper;
            }

            public async Task Handle(UpdateAttrValueCommand request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.AttributeValue.IsExistsAsync(new ObjectID<E.AttributeValue>(request.ID)))
                {
                    var name = await _repoWrapper.AttributeValue.GetByIdAsync(new ObjectID<E.AttributeValue>(request.ID));
                    _mapper.Map(request.attributeValue, name);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new AttributeValueNotFoundException(request.ID);
            }
        }

    }
}
