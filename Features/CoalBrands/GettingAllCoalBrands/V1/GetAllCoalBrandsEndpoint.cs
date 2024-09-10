using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.CoalBrands.GettingAllCoalBrands.V1;

public class GetAllCoalBrandsEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/brands", HandleRequestAsync)
            .WithTags("brands");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromServices] GetAllCoalBrandsCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await commandHandler.HandleAsync(c)
            .MapError(error => Results.BadRequest(error))
            .Map(brands => Results.Ok(brands));

        return result.IsSuccess ? result.Value : result.Error;
    }
}
