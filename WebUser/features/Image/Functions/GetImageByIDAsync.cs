using AutoMapper;
using MediatR;
using E=WebUser.Domain.entities;
using WebUser.features.Image.DTO;
using WebUser.features.Image.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;

namespace WebUser.features.Image.Functions
{
    public class GetImageByIDAsync
    {
        //input
        public class GetImageByIDQuery : IRequest<ImageDTO>
        {
            public int Id { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetImageByIDQuery, ImageDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ImageDTO> Handle(GetImageByIDQuery request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Image.IsExistsAsync(new ObjectID<E.Image>(request.Id)))
                {
                    var img = await _repoWrapper.Image.GetByIdAsync(new ObjectID<E.Image>(request.Id));
                    var results = _mapper.Map<ImageDTO>(img);
                    return results;
                }
                throw new ImageNotFoundException(request.Id);
            }
        }
    }
}
