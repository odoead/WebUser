using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.shared.RepoWrapper;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.OrderProduct.DTO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebUser.features.OrderProduct.Functions
{
    public class CreateOrderProduct
    {
        //input
        public class CreateOrderProductCommand : IRequest<OrderProductDTO>
        {
            public int ID { get; set; }
            public E.Order Order { get; set; }
            public int OrderId { get; set; }
            public int Amount { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreateOrderProductCommand, OrderProductDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<OrderProductDTO> Handle(CreateOrderProductCommand request, CancellationToken cancellationToken)
            {
                var attrVal = new E.OrderProduct
                {
                    ID = request.ID,
                    Order = request.Order,
                    Amount = request.Amount,
                    OrderId=request.Order.ID,
                    
                };
                _repoWrapper.OrderProduct.Create(attrVal);
                await _repoWrapper.SaveAsync();
                var results = _mapper.Map<OrderProductDTO>(attrVal);
                return results;
            }
        }

    }
}
