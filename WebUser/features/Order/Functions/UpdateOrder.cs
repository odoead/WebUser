using AutoMapper;
using MediatR;
using Enteties = WebUser.Domain.entities;
using WebUser.features.Order.DTO;
using WebUser.Domain.entities;
using WebUser.features.Order.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebUser.features.AttributeValue.DTO;
using WebUser.shared.RepoWrapper;
using WebUser.features.Order.DTO;

namespace WebUser.features.Order.Functions
{
    public class UpdateOrder
    {
        //input
        public class UpdateCommand : IRequest
        {
            public int ID { get; set; }
            public UpdateOrderDTO Order { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<UpdateCommand>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {

                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.Order.IsExistsAsync(request.ID))
                {
                    var Order = await _repoWrapper.Order.GetByIdAsync(request.ID);
                    _mapper.Map(request.Order, Order);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new OrderNotFoundException(request.ID);
            }
        }

    }
}
