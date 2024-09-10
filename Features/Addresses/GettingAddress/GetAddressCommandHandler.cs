using coal_backend.Models;
using coal_backend.Utils;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.Addresses.GettingAddress;

public record GetAddressCommand(Location Location);

public class GetAddressCommandHandler
{
    private readonly IDocumentSession session;

    private readonly IAddressFetcher addressFetcher;

    public GetAddressCommandHandler(IDocumentSession documentSession, IAddressFetcher addressFetcher) {
        this.session = documentSession;
        this.addressFetcher = addressFetcher;
    }

    public async Task<Result<Address>> HandleAsync(GetAddressCommand command, CancellationToken c) {
        var storedAddress = session.Query<Address>()
            .Where(x => x.Latitude < command.Location.Lat + 0.05 && x.Latitude < command.Location.Lat - 0.05)
            .Where(x => x.Longitude < command.Location.Lon + 0.05 && x.Longitude < command.Location.Lon - 0.05)
            .Where(x => x.DisplayName.Length > 20)
            .FirstOrDefault().AsMaybe()
            .ToResult("Address not found");
        if (storedAddress.IsSuccess) return storedAddress;
        return await addressFetcher.GetAddressAsync(command.Location, c)
            .Tap(x => StoreFetchedAddress(x, c));
    }

    private async Task StoreFetchedAddress(Address address, CancellationToken c) {
        var isAddressStored = 
            await session.Query<Address>().AnyAsync(a => a.DisplayName == address.DisplayName, c);

        if (isAddressStored) return;

        session.Store(address);

        await session.SaveChangesAsync(c);
    }
}
