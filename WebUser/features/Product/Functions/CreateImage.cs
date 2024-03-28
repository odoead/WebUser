using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.shared.RepoWrapper;
using WebUser.features.Image.DTO;

namespace WebUser.features.Product.Functions
{
    public class CreateImage
    {
        //input
        public class CreateImageCommand : IRequest<ImageDTO>
        {
            public int Id { get; set; }
            public byte[] ImageContent { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<CreateImageCommand, ImageDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ImageDTO> Handle(CreateImageCommand request, CancellationToken cancellationToken)
            {
                var img = new E.Image
                {
                    ID = request.Id,
                    ImageContent = request.ImageContent,

                };
                _repoWrapper.Image.Create(img);
                await _repoWrapper.SaveAsync();
                var results = _mapper.Map<ImageDTO>(img);
                return results;
            }
        }

    }
}
