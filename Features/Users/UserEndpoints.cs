using coal_backend.Features.Users.DeletingUser.V1;
using coal_backend.Features.Users.GettingAllUsers.V1;
using coal_backend.Features.Users.LoginingUser.V1;
using coal_backend.Features.Users.RegisteringUser.V1;

namespace coal_backend.Features.Users;

public class UserEndpoints
{
    public const string ResourceName = "users";

    public static class V1
    {
        public static void Map(WebApplication app)
        {
            RegisterUserEndpoint.Map(app);

            LoginUserEndpoint.Map(app);

            GetAllUsersEndpoint.Map(app);

            DeleteUserEndpoint.Map(app);
        }
    }
}
