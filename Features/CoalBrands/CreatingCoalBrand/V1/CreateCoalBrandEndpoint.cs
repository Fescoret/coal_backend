using coal_backend.Utils.FluentValidatorEx;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.CoalBrands.CreatingCoalBrand.V1;

public class CreateCoalBrandEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/brands/create", HandleRequestAsync)
        .WithTags("brands");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromServices] CreateCoalBrandCommandHandler commandHandler,
        [FromServices] CreateCoalBrandRequestValidator validator,
        [FromBody] CreateCoalBrandRequest request,
        CancellationToken c
    )
    {
        var result = await validator.ValidateForResult(request)
            .MapError(validationResult =>
                Results.BadRequest(validationResult.ToDictionary()))
            .Bind(x => commandHandler.HandleAsync(new(request.CompanyId, request.Name, request.Price, request.Description), c)
                .MapError(error => Results.BadRequest(error))
                .Map(() => Results.Created("/brands", null))
            );

        return result.IsSuccess ? result.Value : result.Error;
    }

    public class CreateCoalBrandRequest
    {
        public Guid CompanyId { get; set; }

        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public string Description { get; set; } = null!;
    }
}
