using coal_backend.Models;
using CSharpFunctionalExtensions;

namespace coal_backend.Utils;

public class RouteResponse
{
    public List<Route> Routes { get; set; }
}

public class Route
{
    public double Distance { get; set; }
}

public interface IRouteFetcher
{
    Task<Result<ShortestRoute>> GetRoutesAsync(Address from, Address to, CancellationToken c);
}

public class OSRMRouteFetcher : IRouteFetcher
{
    private readonly HttpClient client;

    private readonly ILogger<OSRMRouteFetcher> logger;

    public OSRMRouteFetcher(IHttpClientFactory httpClientFactory, ILogger<OSRMRouteFetcher> logger)
    {
        this.logger = logger;

        client = httpClientFactory.CreateClient("osrm");
    }

    public async Task<Result<ShortestRoute>> GetRoutesAsync(Address from, Address to, CancellationToken c)
    {
        double distance = 0;

        var response = await client.GetAsync(FormatRequestString(from, to), c);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<RouteResponse>();

            logger.LogDebug("Got a contetnt from API: {@Content}", content);

            if (content is not { })
            {
                return Result.Failure<ShortestRoute>("Could not parse content");
            }

            content.Routes.ForEach(route =>
            {
                if (route.Distance > distance) distance = route.Distance;
            });

            var route = ShortestRoute.Create(distance, from.DisplayName, to.DisplayName);

            return route;
        }

        return Result.Failure<ShortestRoute>("There is no way");

        static string FormatRequestString(Address from, Address to) =>
            $"{from.Longitude},{from.Latitude};{to.Longitude},{to.Latitude}?overview=false";
    }
}
