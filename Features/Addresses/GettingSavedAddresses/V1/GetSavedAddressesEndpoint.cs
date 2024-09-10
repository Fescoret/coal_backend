using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Addresses.GettingSavedAddresses.V1;

public class GetSavedAddressesEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/addresses", HandleRequestAsync)
            .WithTags("addresses");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromServices] GetSavedAddressesCommandHandler commandHandler,
        CancellationToken c
    )
    {
        return await commandHandler.HandleAsync(c)
            .MapError(error => Results.BadRequest(error))
            .Map(addresses => Results.Ok(addresses))
            .Match(x => x, e => e);
    }
}
