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


            public Handler(DB_Context context)
            {
                dbcontext = context;

            }

            public async Task<PointDTO> Handle(GetPointByIDQuery request, CancellationToken cancellationToken)
            {
                if (await dbcontext.Points.AnyAsync(q => q.ID == request.Id, cancellationToken: cancellationToken))
                {
                    var point = await dbcontext.Points.FirstOrDefaultAsync(q => q.ID == request.Id, cancellationToken: cancellationToken);
                    var results = new PointDTO
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
                    };
                    return results;
                }
                throw new PointNotFoundException(request.Id);
            }
        }
    }
}
