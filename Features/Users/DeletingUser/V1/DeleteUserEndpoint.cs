using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Users.DeletingUser.V1;

public class DeleteUserEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapDelete($"/users/{{user-id}}/delete", HandleRequestAsync)
            .WithTags("users");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromRoute(Name = "user-id")] Guid userId,
        [FromServices] DeleteUserCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await commandHandler.HandleAsync(new(userId), c)
                .MapError(error => Results.BadRequest(error))
                .Map(() => Results.Ok("Deleted successfully")
            );

        return result.IsSuccess ? result.Value : result.Error;
    }
}