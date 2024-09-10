using coal_backend.Utils.FluentValidatorEx;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Users.LoginingUser.V1;

public class LoginUserEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/auth/signin", HandleRequestAsync)
            .WithTags("users");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginUserRequestValidator reqValidator,
        [FromServices] LoginUserResponseValidator resValidator,
        [FromServices] LoginUserCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await reqValidator.ValidateForResult(request)
            .MapError(validationError =>
                Results.BadRequest(validationError.ToDictionary())
            )
            .Bind(x => commandHandler.HandleAsync(new(x.Email, x.Password), c)
                .MapError(error => Results.BadRequest(error))
                .Bind(async response => await resValidator.ValidateForResult(response)
                    .MapError(error => Results.Problem("Response isn't valid"))
                    .Map(response => Results.Ok(response)))
            );

        return result.IsSuccess ? result.Value : result.Error;
    }

    public class LoginUserRequest
    {
        public string Email { get; init; } = null!;

        public string Password { get; init; } = null!;
    }

    public class LoginUserResponse
    {
        public string Email { get; init; } = null!;

        public string Token { get; init; } = null!;
    }
}
