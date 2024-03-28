using AutoMapper;
using MediatR;
using E= WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebUser.features.AttributeValue.DTO;
using WebUser.shared.RepoWrapper;
using WebUser.features.Cart.DTO;
using WebUser.features.Cart.Exceptions;
using WebUser.shared;

namespace WebUser.features.Cart.functions
{
    public class UpdateCart
    {
        //input
        public class UpdateCartCommand : IRequest
        {
            public int ID { get; set; }
            public CartUpdateDTO cart { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<UpdateCartCommand>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper repoWrapper, IMapper mapper)
            {

                _repoWrapper = repoWrapper;
                _mapper = mapper;
            }

            public async Task Handle(UpdateCartCommand request, CancellationToken cancellationToken)
            {
                 if (await _repoWrapper.Cart.IsExistsAsync(new ObjectID<E.Cart> (request.ID)))
                {
                    var name = await _repoWrapper.Cart.GetByIdAsync(new ObjectID<E.Cart>(request.ID));
                    _mapper.Map(request.cart, name);
                    await _repoWrapper.SaveAsync();
                    
                }
                else
                    throw new CartNotFoundException(request.ID);
            }
        }

    }
}
