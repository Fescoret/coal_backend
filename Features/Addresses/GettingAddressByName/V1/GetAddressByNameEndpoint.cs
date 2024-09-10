using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace coal_backend.Features.Addresses.GettingAddressByName.V1;

public class GetAddressByNameEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet($"/addresses/search={{place_name}}", HandleRequestAsync)
        .WithTags("addresses");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromRoute(Name = "place_name")] string searchString,
        [FromServices] GetAddressByNameCommandHandler commandHandler,
        CancellationToken c
    )
    {
        string formatedSearch = HttpUtility.UrlDecode(searchString)
            .ToLower()
            .Replace(" ", "+");
        while (formatedSearch.Contains("++")) {
            formatedSearch = formatedSearch.Replace("++", "+");
        }
        if(formatedSearch == "+" || formatedSearch == "") return Results.Ok("fetched");
        return await commandHandler.HandleAsync(new(formatedSearch), c)
            .MapError(error => Results.BadRequest(error))
            .Map(addresses => Results.Ok(addresses))
            .Match(x => x, e => e);
    }
}
