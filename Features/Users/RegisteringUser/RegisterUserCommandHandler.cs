using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.Users.RegisteringUser;

public record RegisterUserCommand(string FirstName, string LastName, string EmailAddress, string Password);

public class RegisterUserCommandHandler
{
    private readonly IDocumentSession session;

    public RegisterUserCommandHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public async Task<Result> HandleAsync(RegisterUserCommand command, CancellationToken c)
    {
        var hashedPassword = BC.HashPassword(command.Password);
        return await User.Create(command.FirstName, command.LastName, command.EmailAddress, hashedPassword)
            .Tap(user => session.Store(user))
            .Tap(() => session.SaveChangesAsync(c));
    }
}
