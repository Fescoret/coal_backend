using coal_backend.Models;
using coal_backend.Utils;
using CSharpFunctionalExtensions;

namespace coal_backend.Features.Addresses.GettingAddressByName;

public record GetAddressByNameCommand(string searchString);

public class GetAddressByNameCommandHandler
{
    private readonly IAddressFetcher addressFetcher;

    public GetAddressByNameCommandHandler(IAddressFetcher addressFetcher)
    {
        this.addressFetcher = addressFetcher;
    }

    public async Task<Result<IEnumerable<Address>>> HandleAsync(GetAddressByNameCommand command, CancellationToken c)
    {
        return await addressFetcher.GetAddressByNameAsync(command.searchString, c);
    }
}
