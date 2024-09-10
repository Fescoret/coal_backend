using coal_backend.Utils.FluentValidatorEx;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Addresses.SaveAddress.V1;

public class SaveAddressEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/addresses/save", HandleRequestAsync)
        .WithTags("addresses");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromServices] SaveAddressCommandHandler commandHandler,
        [FromServices] SaveAddressRequestValidator validator,
        [FromBody] SaveAddressRequest request,
        CancellationToken c
    )
    {
        var result = await validator.ValidateForResult(request)
            .MapError(validationResult =>
                Results.BadRequest(validationResult.ToDictionary()))
            .Bind(x => commandHandler.HandleAsync(new(request.DisplayName, request.Lat, request.Lon), c)
                .MapError(error => Results.BadRequest(error))
                .Map(() => Results.Created("/addresses", null))
            );

        return result.IsSuccess ? result.Value : result.Error;
    }

    public class SaveAddressRequest
    {
        public string DisplayName { get; set; } = null!;

        public double Lat { get; set; }

        public double Lon { get; set; }
    }
}
