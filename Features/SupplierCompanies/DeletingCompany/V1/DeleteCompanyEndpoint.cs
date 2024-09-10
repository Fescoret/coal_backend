using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.SupplierCompanies.DeletingCompany.V1;

public class DeleteCompanyEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapDelete($"/companies/{{company-id}}/delete", HandleRequestAsync)
            .WithTags("companies");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromRoute(Name = "company-id")] Guid companyId,
        [FromServices] DeleteCompanyCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await commandHandler.HandleAsync(new(companyId), c)
                .MapError(error => Results.BadRequest(error))
                .Map(() => Results.Ok("Deleted successfully")
            );

        return result.IsSuccess ? result.Value : result.Error;
    }
}
