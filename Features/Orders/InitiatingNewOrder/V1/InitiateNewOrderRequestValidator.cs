using FluentValidation;

namespace coal_backend.Features.Orders.InitiatingNewOrder.V1;

public class InitiateNewOrderRequestValidator : AbstractValidator<InitiateNewOrderEndpoint.InitiateNewOrderRequest>
{
    public InitiateNewOrderRequestValidator()
    {
        RuleFor(x => x.UserEmail).EmailAddress();
        RuleFor(x => x.AddressName).NotEmpty();
        RuleFor(x => x.CoalBrandId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
