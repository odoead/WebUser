using AutoMapper;
using MediatR;
using WebUser.Data;
using WebUser.features.Point.DTO;
using E = WebUser.Domain.entities;

namespace WebUser.features.Point.Functions
{
    public class CreatePoint
    {
        //input
        public class CreatePointCommand : IRequest<PointDTO>
        {
            public int Value { get; set; }
            public bool isExpirable { get; set; }
            public DateTime ExpireDate { get; set; }
            public E.User User { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<CreatePointCommand, PointDTO>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<PointDTO> Handle(CreatePointCommand request, CancellationToken cancellationToken)
            {
                var point = new E.Point
                {
                    CreateDate = DateTime.UtcNow,
                    ExpireDate = request.ExpireDate,
                    User = request.User,
                    IsExpirable = request.isExpirable,
                    Value = request.Value,
                    BalanceLeft = request.Value,
                    IsUsed = false,
                };
                await dbcontext.Points.AddAsync(point, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);
                var results = mapper.Map<PointDTO>(point);
                return results;
            }
        }
    }
}
