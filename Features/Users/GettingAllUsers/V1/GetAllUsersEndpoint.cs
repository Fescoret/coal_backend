using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Users.GettingAllUsers.V1;

public class GetAllUsersEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/users", HandleRequestAsync)
            .WithTags("users");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromServices] GetAllUsersCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await commandHandler.HandleAsync(c)
            .MapError(error => Results.BadRequest(error))
            .Map(users => Results.Ok(users));

        return result.IsSuccess ? result.Value : result.Error;
    }
}
