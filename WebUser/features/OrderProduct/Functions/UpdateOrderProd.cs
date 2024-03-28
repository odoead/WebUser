using AutoMapper;
using MediatR;
using Enteties = WebUser.Domain.entities;
using WebUser.features.Category.DTO;
using WebUser.Domain.entities;
using WebUser.features.Category.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebUser.features.AttributeValue.DTO;
using WebUser.shared.RepoWrapper;
using WebUser.features.OrderProduct.Exceptions;
using WebUser.features.OrderProduct.DTO;

namespace WebUser.features.OrderProduct.Functions
{
    public class UpdateOrderProduct
    {
        //input
        public class UpdateOrderProdCommand : IRequest
        {
            public int ID { get; set; }
            public UpdateOrderProdDTO orderProduct { get; set; }

        }
        //handler
        public class Handler : IRequestHandler<UpdateOrderProdCommand>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {

                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task Handle(UpdateOrderProdCommand request, CancellationToken cancellationToken)
            {
                if (await _repoWrapper.OrderProduct.IsExistsAsync(request.ID))
                {
                    var OrderProduct = await _repoWrapper.OrderProduct.GetByIdAsync(request.ID);
                    _mapper.Map(request.orderProduct, OrderProduct);
                    await _repoWrapper.SaveAsync();

                }
                else
                    throw new OrderProductNotFoundException(request.ID);
            }
        }

    }
}
