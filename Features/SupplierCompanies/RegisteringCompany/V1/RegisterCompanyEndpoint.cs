using Microsoft.AspNetCore.Mvc;
using CSharpFunctionalExtensions;
using coal_backend.Utils.FluentValidatorEx;

namespace coal_backend.Features.SupplierCompanies.RegisteringCompany.V1;

public class RegisterCompanyEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/companies/create", HandleRequestAsync)
        .WithTags("companies");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromServices] RegisterCompanyCommandHandler commandHandler,
        [FromServices] RegisterCompanyRequestValidator validator,
        [FromBody] RegisterCompanyRequest request,
        CancellationToken c
    )
    {
        var result = await validator.ValidateForResult(request)
            .MapError(validationResult =>
                Results.BadRequest(validationResult.ToDictionary()))
            .Bind(x => commandHandler.HandleAsync(new(request.CompanyName, request.Description, request.Location), c)
                .MapError(error => Results.BadRequest(error))
                .Map(() => Results.Created("/companies", null))
            );

        return result.IsSuccess ? result.Value : result.Error;
    }

    public class RegisterCompanyRequest
    {
        public string CompanyName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Guid Location { get; set; }
    }
}
