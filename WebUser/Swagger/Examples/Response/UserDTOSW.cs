using Swashbuckle.AspNetCore.Filters;

namespace WebUser.features.User.DTO
{
    public class UserDTOSW : IExamplesProvider<UserDTO>
    {
        public UserDTO GetExamples() =>
            new()
            {
                DateCreated = DateTime.UtcNow,
                FirstName = "John",
                LastName = "Doe",
                Id = "uerid",
                UserName = "johndoe",
                Email = "John.Doe@gmail.com",
            };
    }
}
