using coal_backend.Utils.FluentValidatorEx;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace coal_backend.Features.ShortestRoutes.MakeAndSaveRoutes.V1;

public class MakeAndSaveRoutesEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet($"/routes/{{from_location}}/{{to_location}}", HandleRequestAsync)
            .WithTags("routes");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromRoute(Name = "from_location")] string FromLocation,
        [FromRoute(Name = "to_location")] string ToLocation,
        [FromServices] MakeAndSaveRoutesCommandHandler commandHandler,
        CancellationToken c
    )
    {
        string UserAddressName = HttpUtility.UrlDecode(ToLocation);
        string CompanyAddressName = HttpUtility.UrlDecode(FromLocation);
        var result = await commandHandler.HandleAsync(new(UserAddressName, CompanyAddressName), c)
                .MapError(error => Results.BadRequest(error))
                .Map(route => Results.Ok(route));

        return result.IsSuccess ? result.Value : result.Error;
    }
}
