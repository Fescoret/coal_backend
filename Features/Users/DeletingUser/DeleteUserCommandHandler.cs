using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.Users.DeletingUser;

public record DeleteUserCommand(Guid UserId);

public class DeleteUserCommandHandler
{
    private readonly IDocumentSession session;

    public DeleteUserCommandHandler(IDocumentSession documentSession)
    {
        this.session = documentSession;
    }

    public async Task<Result> HandleAsync(DeleteUserCommand command, CancellationToken c)
    {
        return await session.Load<User>(command.UserId).AsMaybe()
            .ToResult("User with such ID does not exist")
            .Tap(user => session.Delete(user))
            .Tap(() => session.SaveChangesAsync(c));
    }
}
