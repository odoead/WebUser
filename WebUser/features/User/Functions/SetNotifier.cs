namespace WebUser.features.User.Functions;

using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.Domain.entities;

public class SetNotifier
{
    public class SetNotifierCommand : IRequest
    {
        public ClaimsPrincipal UserClaims { get; set; }
        public int ProductId { get; set; }
    }

    public class Handler : IRequestHandler<SetNotifierCommand>
    {
        private readonly DB_Context dbcontext;
        private readonly UserManager<User> userManager;
        public Handler(DB_Context context, UserManager<User> userManager)
        {
            dbcontext = context;
            this.userManager = userManager;
        }

        public async Task Handle(SetNotifierCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

            if (
                await dbcontext.RequestNotifications.AnyAsync(
                    q => q.UserID != userId && q.ProductID != request.ProductId,
                    cancellationToken: cancellationToken
                )
            )
            {
                await dbcontext.RequestNotifications.AddAsync(
                    new ProductUserNotificationRequest { ProductID = request.ProductId, UserID = userId },
                    cancellationToken
                );
                await dbcontext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
