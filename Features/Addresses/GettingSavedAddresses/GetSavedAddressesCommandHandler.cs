using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.Addresses.GettingSavedAddresses;

public class GetSavedAddressesCommandHandler
{
    private readonly IQuerySession session;

    public GetSavedAddressesCommandHandler(IQuerySession documentSession)
    {
        this.session = documentSession;
    }

    public async Task<Result<IReadOnlyList<Address>>> HandleAsync(CancellationToken c)
    {
        return (await session.Query<Address>().ToListAsync(c)).AsMaybe()
            .ToResult("Server Error: Addresses table does'nt exist")
            .Bind(addresses => addresses.IsEmpty()
                        ? Result.Failure<IReadOnlyList<Address>>("There is no addresses")
                        : Result.Success(addresses));
    }
}
