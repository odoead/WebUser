using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebUser.features.CartItem.DTO;
using WebUser.shared.RepoWrapper;
using WebUser.features.CartItem.Exceptions;
using WebUser.shared;

namespace WebUser.features.CartItem.functions
{
    public class UpdateCartItem
    {
        //input
        public class UpdateCartItemCommand : IRequest
        {
            public int ID { get; set; }
            public CartItemUpdateDTO cartItem { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<UpdateCartItemCommand>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper repoWrapper, IMapper mapper)
            {

                _repoWrapper = repoWrapper;
                _mapper = mapper;
            }

            public async Task Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
            {
                 if (await _repoWrapper.CartItem.IsExistsAsync(new ObjectID<E.CartItem>(request.ID)))
                {
                    var item = await _repoWrapper.CartItem.GetByIdAsync(new ObjectID<E.CartItem>(request.ID));
                    _mapper.Map(request.cartItem, item);
                    await _repoWrapper.SaveAsync();
                    
                }
                else
                    throw new CartItemNotFoundException(request.ID);
            }
        }

    }
}
