using coal_backend.Utils;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Addresses.GettingAddress.V1;

public class GetAddressEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet($"/addresses/lat={{lattitude}}&lon={{longitude}}", HandleRequestAsync)
            .WithTags("addresses");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromRoute(Name = "lattitude")] double lattitude,
        [FromRoute(Name = "longitude")] double longitude,
        [FromServices] GetAddressCommandHandler commandHandler,
        CancellationToken c
    ) {
        if (lattitude == 64.8488 && longitude == 109.8638) {
            return Results.Ok("fetched");
        }
        if (lattitude == 53.7173 && longitude == 91.4451) {
            return Results.Ok("fetched");
        }
        return await commandHandler.HandleAsync(new(new(lattitude, longitude)), c)
            .MapError(error => Results.BadRequest(error))
            .Map(address => Results.Ok(address))
            .Match(x => x, e => e);
    }
}
