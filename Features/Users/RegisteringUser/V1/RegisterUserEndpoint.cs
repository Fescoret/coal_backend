using coal_backend.Utils.FluentValidatorEx;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Users.RegisteringUser.V1;

public static class RegisterUserEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/auth/signup", HandleRequestAsync)
            .WithTags("users");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserRequestValidator validator,
        [FromServices] RegisterUserCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await validator.ValidateForResult(request)
            .MapError(validationError =>
                Results.BadRequest(validationError.ToDictionary())
            )
            .Bind(x => commandHandler.HandleAsync(new(x.FirstName, x.LastName, x.Email, x.Password), c)
                .MapError(error => Results.BadRequest(error))
                .Map(() => Results.Created("/", null))
            );

        return result.IsSuccess ? result.Value : result.Error;
    }

    public class RegisterUserRequest
    {
        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string Email { get; init; } = null!;

        public string Password { get; init; } = null!;
    }
}
