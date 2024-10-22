using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Point.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Point.Functions
{
    public class GetAllPoints
    {
        //input
        public class GetAllPointsQuery : IRequest<PagedList<PointDTO>>
        {
            public PointRequestParameters Parameters { get; set; }

            public GetAllPointsQuery(PointRequestParameters parameters)
            {
                Parameters = parameters;
            }
        }

        //handler
        public class Handler : IRequestHandler<GetAllPointsQuery, PagedList<PointDTO>>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<PagedList<PointDTO>> Handle(GetAllPointsQuery request, CancellationToken cancellationToken)
            {
                var data = dbcontext.Points.AsQueryable();
                var srcPoints = await data.Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                    .Take(request.Parameters.PageSize)
                    .ToListAsync(cancellationToken);

                var dtoPoints = srcPoints
                    .Select(point => new PointDTO
                    {
                        ID = point.ID,
                        Value = point.Value,
                        BalanceLeft = point.BalanceLeft,
                        isExpirable = point.IsExpirable,
                        IsUsed = point.IsUsed,
                        IsActive = point.IsUsed,
                        CreateDate = point.CreateDate,
                        ExpireDate = point.ExpireDate,
                        UserID = point.UserID,
                        OrderID = point.OrderID,
                    })
                    .ToList();

                var pagedList = PagedList<PointDTO>.PaginateList(
                    source: dtoPoints,
                    totalCount: await data.CountAsync(cancellationToken: cancellationToken),
                    pageNumber: request.Parameters.PageNumber,
                    pageSize: request.Parameters.PageSize
                );

                return pagedList;
            }
        }
    }
}
