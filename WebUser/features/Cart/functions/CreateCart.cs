using AutoMapper;
using MediatR;
using WebUser.features.Cart.DTO;
using WebUser.shared.RepoWrapper;
using E = WebUser.Domain.entities;

namespace WebUser.features.Cart.functions
{
    public class CreateCart
    {
        //input
        public class CreateCartCommand : IRequest<CartDTO>
        {
            public E.User User { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreateCartCommand, CartDTO>
        {
            private IRepoWrapper _RepoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper RepoWrapper, IMapper mapper)
            {
                _RepoWrapper = RepoWrapper;
                _mapper = mapper;
            }

            public async Task<CartDTO> Handle(CreateCartCommand request, CancellationToken cancellationToken)
            {
                var cart = new E.Cart
                {
                    User = request.User,
                    items = null,
                    UserId = request.User.Id

                };
                _RepoWrapper.Cart.Create(cart);
                await _RepoWrapper.SaveAsync();
                var results = _mapper.Map<CartDTO>(cart);
                return results;
            }
        }

    }
}
