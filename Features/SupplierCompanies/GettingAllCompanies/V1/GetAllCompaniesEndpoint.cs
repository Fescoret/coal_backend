using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.SupplierCompanies.GettingAllCompanies.V1;

public class GetAllCompaniesEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/companies", HandleRequestAsync)
            .WithTags("companies");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromServices] GetAllCompaniesCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await commandHandler.HandleAsync(c)
            .MapError(error => Results.BadRequest(error))
            .Map(users => Results.Ok(users));

        return result.IsSuccess ? result.Value : result.Error;
    }
}
