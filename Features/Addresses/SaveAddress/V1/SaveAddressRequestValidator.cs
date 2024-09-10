using FluentValidation;

namespace coal_backend.Features.Addresses.SaveAddress.V1;

public class SaveAddressRequestValidator: AbstractValidator<SaveAddressEndpoint.SaveAddressRequest>
{
    public SaveAddressRequestValidator()
    {
        RuleFor(x => x.DisplayName).NotEmpty();
        RuleFor(x => x.Lat).NotNull();
        RuleFor(x => x.Lon).NotNull();
    }
}
