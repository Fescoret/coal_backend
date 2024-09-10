using coal_backend.Models;
using coal_backend.Utils;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.ShortestRoutes.MakeAndSaveRoutes;

public record MakeAndSaveRoutesCommand(string UserAddressName, string CompanyAddressName);

public class MakeAndSaveRoutesCommandHandler
{
    private readonly IDocumentSession session;

    private readonly IRouteFetcher fetcher;

    public MakeAndSaveRoutesCommandHandler(IDocumentSession documentSession, IRouteFetcher routeFetcher)
    {
        this.session = documentSession;
        this.fetcher = routeFetcher;
    }

    private static async Task<Result<ShortestRoute>> MakeRouteAsync(IDocumentSession session, IRouteFetcher fetcher, MakeAndSaveRoutesCommand command, CancellationToken c)
    {
        var To = session.Query<Address>()
            .Where(x => x.DisplayName.Contains(command.UserAddressName))
            .FirstOrDefault();

        if (To == null) return Result.Failure<ShortestRoute>("There is no address with this name");

        var From = session.Query<Address>()
            .Where(x => x.DisplayName.Contains(command.CompanyAddressName))
            .FirstOrDefault();

        if (From == null) return Result.Failure<ShortestRoute>("There is no address with this name");

        await Task.Delay(5000);

        return await fetcher.GetRoutesAsync(From, To, c)
            .Tap(route => session.Store(route));
    }

    public async Task<Result<ShortestRoute>> HandleAsync(MakeAndSaveRoutesCommand command, CancellationToken c)
    {
        if (command.UserAddressName == "Выбрать") return Result.Success(new ShortestRoute());
        if (command.CompanyAddressName == "Выбрать") return Result.Success(new ShortestRoute());
        return await session.Query<ShortestRoute>()
            .Where(x => x.To == command.UserAddressName && x.From == command.CompanyAddressName)
            .ToList()
            .AsMaybe().ToResult("ShortestRoute table doesn't exist")
            .Bind(routes => routes.Any()
                ? Result.Success(routes.First())
                : MakeRouteAsync(session, fetcher, command, c).Result)
            .Tap(() => session.SaveChangesAsync(c));
        /*return await MakeRouteAsync(session, fetcher, command, c)
            .Tap(() => session.SaveChangesAsync(c));*/
    }
}
