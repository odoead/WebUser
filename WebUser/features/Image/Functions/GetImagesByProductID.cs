using AutoMapper;
using MediatR;
using WebUser.features.Image.DTO;
using WebUser.features.Image.Exceptions;
using WebUser.features.Product.Exceptions;
using WebUser.shared;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Image.Functions
{
    public class GetImagesByProductID
    {
        public class GetImageByProductIDQuery : IRequest<ICollection<ImageDTO>>
        {
            public int ProductId { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<GetImageByProductIDQuery, ICollection<ImageDTO>>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<ICollection<ImageDTO>> Handle(GetImageByProductIDQuery request, CancellationToken cancellationToken)
            {
                if(!(await _repoWrapper.Product.IsExistsAsync(new ObjectID<E.Product>(request.ProductId))))
                    throw new ProductNotFoundException(request.ProductId);
                
                    var imgs = await _repoWrapper.Image.GetByProductIdAsync(new ObjectID<E.Product>(request.ProductId));
                if(imgs == null)
                {
                    throw new ImageNotFoundException(-1);
                }
                    var results = _mapper.Map<ICollection<ImageDTO>>(imgs);
                    return results;
                
            }
        }
    }
}
