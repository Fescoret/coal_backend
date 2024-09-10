using coal_backend.Features.Users.RegisteringUser;
using coal_backend.Utils.FluentValidatorEx;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Orders.InitiatingNewOrder.V1;

public class InitiateNewOrderEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/orders/create", HandleRequestAsync)
            .WithTags("orders");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromServices] InitiateNewOrderCommandHandler commandHandler,
        [FromServices] RegisterUserCommandHandler registerUserCommandHandler,
        [FromServices] InitiateNewOrderRequestValidator validator,
        [FromBody] InitiateNewOrderRequest request,
        CancellationToken c
    )
    {
        if ( !string.IsNullOrEmpty(request.UserPassword) && 
            !string.IsNullOrEmpty(request.UserLastName) && 
            !string.IsNullOrEmpty(request.UserFirstName))
        {
            var user = await registerUserCommandHandler.HandleAsync(new(
                request.UserFirstName,
                request.UserLastName,
                request.UserEmail,
                request.UserPassword), c);
        }
        var result = await validator.ValidateForResult(request)
            .MapError(validationResult =>
                Results.BadRequest(validationResult.ToDictionary()))
            .Bind(x => commandHandler.HandleAsync(new(request.UserEmail, request.AddressName, request.CoalBrandId, request.Amount), c)
                .MapError(error => Results.BadRequest(error))
                .Map(() => Results.Created("/profile", null))
            );

        return result.IsSuccess ? result.Value : result.Error;
    }

    public class InitiateNewOrderRequest
    {
        public string UserEmail { get; init; } = null!;

        public string? UserPassword { get; init; }

        public string? UserFirstName { get; init; }

        public string? UserLastName { get; init; }

        public string AddressName { get; init; } = null!;

        public Guid CoalBrandId { get; init; }

        public double Amount { get; init; }
    }
}
