using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.Addresses.SaveAddress;

public record SaveAddressCommand(string DisplayName, double Lat, double Lon);

public class SaveAddressCommandHandler
{
    private readonly IDocumentSession session;

    public SaveAddressCommandHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public async Task<Result> HandleAsync(SaveAddressCommand command, CancellationToken c)
    {
        return await Address.Create(command.DisplayName, command.Lat, command.Lon)
            .Tap(address => session.Store(address))
            .Tap(() => session.SaveChangesAsync(c));
    }
}
