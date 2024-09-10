using coal_backend.Models;
using coal_backend.Utils;
using CSharpFunctionalExtensions;
using Marten;
using static coal_backend.Features.Users.LoginingUser.V1.LoginUserEndpoint;

namespace coal_backend.Features.Users.LoginingUser;

public record LoginUserCommand(string EmailAddress, string Password);

public class LoginUserCommandHandler
{
    private readonly IQuerySession session;

    private readonly TokenFactory tokenFactory;

    public LoginUserCommandHandler(IQuerySession documentSession, TokenFactory tokenFactoryX)
    {
        this.session = documentSession;
        this.tokenFactory = tokenFactoryX;
    }

    public async Task<Result<LoginUserResponse>> HandleAsync(LoginUserCommand command, CancellationToken c)
    {
        return (await session.Query<User>()
            .Where(x => x.Email == command.EmailAddress)
            .FirstOrDefaultAsync(c)).AsMaybe()
            .ToResult("User with such Email does not exist")
            .Bind(user =>
            {
                return user.Login(command.Password)
                    ? Result.Success(user)
                    : Result.Failure<User>("Incorrect password");
            })
            .Map(user =>
            {
                LoginUserResponse response = new() { Email = command.EmailAddress, Token = tokenFactory.CreateToken(user) };
                return response;
            });
    }
}
