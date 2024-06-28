using AutoMapper;
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
            public string? UserName { get; init; }
            public string? Password { get; init; }
            public string? Email { get; init; }
            public string? PhoneNumber { get; init; }
            public ICollection<string>? Roles { get; init; }
        }

        //handler
        public class Handler : IRequestHandler<CreateUserCommand, IdentityResult>
        {
            private readonly IMapper mapper;
            private readonly UserManager<E.User> _userManager;

            public Handler(IMapper mapper, UserManager<E.User> userManager)
            {
                this.mapper = mapper;
                _userManager = userManager;
            }

            public async Task<IdentityResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = mapper.Map<E.User>(request);
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, request.Roles);
                }
                return result;
            }
        }
    }
}
