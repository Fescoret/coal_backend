using FluentValidation;

namespace coal_backend.Features.SupplierCompanies.RegisteringCompany.V1;

public class RegisterCompanyRequestValidator : AbstractValidator<RegisterCompanyEndpoint.RegisterCompanyRequest>
{
    public RegisterCompanyRequestValidator()
    {
        RuleFor(x => x.CompanyName).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Location).NotEmpty();
    }
}
