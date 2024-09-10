using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.SupplierCompanies.GettingCompanyById.V1;

public class GetCompanyByIdEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet($"/companies/{{company-id}}", HandleRequestAsync)
            .WithTags("companies");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromRoute(Name = "company-id")] Guid companyId,
        [FromServices] GetCompanyByIdCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await commandHandler.HandleAsync(new(companyId), c)
                .MapError(error => Results.BadRequest(error))
                .Map(company => Results.Ok(company));

        return result.IsSuccess ? result.Value : result.Error;
    }
}
