using MediatR;
using Microsoft.AspNetCore.Identity;
using E = WebUser.Domain.entities;

namespace WebUser.features.User.Functions
{
    public class CreateUser
    {
        //input
        public class CreateUserCommand : IRequest<IdentityResult>
        {
            public string? FirstName { get; init; }
            public string? LastName { get; init; }
            public string? Password { get; init; }
            public string? Email { get; init; }
            public string? PhoneNumber { get; init; }
        }

        //handler
        public class Handler : IRequestHandler<CreateUserCommand, IdentityResult>
        {
            private readonly UserManager<E.User> _userManager;

            public Handler(UserManager<E.User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<IdentityResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = new E.User
                {
                    Email = request.Email,
                    LastName = request.LastName,
                    FirstName = request.FirstName,
                    DateCreated = DateTime.UtcNow,
                    UserName = request.Email,
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, new string[] { "User" });
                }
                return result;
            }
        }
    }
}
