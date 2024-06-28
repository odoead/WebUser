using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Point.DTO;
using WebUser.features.Point.Exceptions;

namespace WebUser.features.Point.Functions
{
    public class GetPointByID
    {
        //input
        public class GetPointByIDQuery : IRequest<PointDTO>
        {
            public int Id { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetPointByIDQuery, PointDTO>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<PointDTO> Handle(GetPointByIDQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Points.AnyAsync(q => q.ID == request.Id, cancellationToken: cancellationToken))
                {
                    var point = await dbcontext.Points.FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken);
                    var results = mapper.Map<PointDTO>(point);
                    return results;
                }
                throw new PointNotFoundException(request.Id);
            }
        }
    }
}
