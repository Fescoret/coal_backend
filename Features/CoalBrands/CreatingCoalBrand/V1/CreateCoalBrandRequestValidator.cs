using FluentValidation;

namespace coal_backend.Features.CoalBrands.CreatingCoalBrand.V1;

public class CreateCoalBrandRequestValidator : AbstractValidator<CreateCoalBrandEndpoint.CreateCoalBrandRequest>
{
    public CreateCoalBrandRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
