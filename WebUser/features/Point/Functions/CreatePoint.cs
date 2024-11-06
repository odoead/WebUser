using MediatR;
using Microsoft.AspNetCore.Identity;
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
            public bool IsExpirable { get; set; }
            public DateTime ExpireDate { get; set; }
            public string UserId { get; set; } = string.Empty;
        }

        //handler
        public class Handler : IRequestHandler<CreatePointCommand, PointDTO>
        {
            private readonly DB_Context dbcontext;
            private readonly UserManager<E.User> userManager;
            public Handler(DB_Context context, UserManager<E.User> userManager)
            {
                dbcontext = context;
                this.userManager = userManager;
            }

            public async Task<PointDTO> Handle(CreatePointCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.UserId);



                var point = new E.Point
                {
                    CreateDate = DateTime.UtcNow,
                    ExpireDate = request.ExpireDate,
                    User = user,
                    IsExpirable = request.IsExpirable,
                    Value = request.Value,
                    BalanceLeft = request.Value,
                    IsUsed = false,
                    UserID = user.Id,
                };
                await dbcontext.Points.AddAsync(point, cancellationToken);
                await dbcontext.SaveChangesAsync(cancellationToken);
                var results = new PointDTO
                {
                    IsActive = true,
                    Value = point.Value,
                    IsUsed = point.IsUsed,
                    ID = point.ID,
                    BalanceLeft = point.BalanceLeft,
                    CreateDate = DateTime.UtcNow,
                    ExpireDate = point.ExpireDate,
                    isExpirable = point.IsExpirable,
                    UserID = point.UserID,
                    OrderID = null,
                };
                return results;
            }
        }
    }
}
