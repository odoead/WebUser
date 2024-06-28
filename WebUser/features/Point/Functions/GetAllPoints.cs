using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.AttributeName.DTO;
using WebUser.features.Point.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Point.Functions
{
    public class GetAllPoints
    {
        //input
        public class GetAllPointsQuery : IRequest<ICollection<PointDTO>>
        {
            public RequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllPointsQuery, ICollection<PointDTO>>
        {
            private readonly DB_Context dbcontext;
            private readonly IMapper mapper;

            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                this.mapper = mapper;
            }

            public async Task<ICollection<PointDTO>> Handle(GetAllPointsQuery request, CancellationToken cancellationToken)
            {
                var points = await dbcontext.Points.ToListAsync(cancellationToken: cancellationToken);
                var results = mapper.Map<ICollection<PointDTO>>(points);
                return PagedList<PointDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
            }
        }
    }
}
