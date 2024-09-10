using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.Users.GettingAllUsers;

public class GetAllUsersCommandHandler
{
    private readonly IQuerySession session;

    public GetAllUsersCommandHandler(IQuerySession documentSession)
    {
        this.session = documentSession;
    }

    public async Task<Result<IReadOnlyList<User>>> HandleAsync(CancellationToken c)
    {
        return (await session.Query<User>().ToListAsync(c)).AsMaybe()
            .ToResult("Server Error: Users table does'nt exist")
            .Bind(users => users.IsEmpty()
                        ? Result.Failure<IReadOnlyList<User>>("There is no users")
                        : Result.Success(users));
    }
}
