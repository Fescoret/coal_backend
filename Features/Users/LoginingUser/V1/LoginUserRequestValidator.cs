using FluentValidation;

namespace coal_backend.Features.Users.LoginingUser.V1;

public class LoginUserRequestValidator : AbstractValidator<LoginUserEndpoint.LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
