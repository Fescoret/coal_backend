using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.CoalBrands.DeletingCoalBrand.V1;

public class DeleteCoalBrandEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapDelete($"/brands/{{brand-id}}/delete", HandleRequestAsync)
        .WithTags("brands");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromRoute(Name = "brand-id")] Guid brandId,
        [FromServices] DeleteCoalBrandCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await commandHandler.HandleAsync(new(brandId), c)
                .MapError(error => Results.BadRequest(error))
                .Map(() => Results.Ok("Deleted successfully")
            );

        return result.IsSuccess ? result.Value : result.Error;
    }
}
