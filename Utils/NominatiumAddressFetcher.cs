using coal_backend.Models;
using CSharpFunctionalExtensions;
using System.Text.Json.Serialization;

namespace coal_backend.Utils;

public class AddressResponse {
    [JsonPropertyName("place_id")]
    public required long PlaceId { get; init; }

    [JsonPropertyName("display_name")]
    public required string DisplayName { get; init; }

    public required double Lat { get; init; }

    public required double Lon { get; init; }
}

public record struct Location(double Lat, double Lon);

public interface IAddressFetcher {
    Task<Result<Address>> GetAddressAsync(Location location, CancellationToken c);

    Task<Result<IEnumerable<Address>>> GetAddressByNameAsync(string searchString, CancellationToken c);
}

public class NominatiumAddressFetcher : IAddressFetcher {
    private readonly HttpClient client;

    private readonly ILogger<NominatiumAddressFetcher> logger;

    public NominatiumAddressFetcher(IHttpClientFactory httpClientFactory, ILogger<NominatiumAddressFetcher> logger) {
        this.logger = logger;

        client = httpClientFactory.CreateClient("nominatium");
    }

    public async Task<Result<Address>> GetAddressAsync(Location location, CancellationToken c) {
        var response = await client.GetAsync(FormatRequestString(location), c);

        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadFromJsonAsync<AddressResponse>();

            logger.LogDebug("Got a contetnt from API: {@Content}", content);

            if (content is not { }) {
                return Result.Failure<Address>("Could not parse content");
            }

            var address = Address.Create(content.DisplayName, content.Lat, content.Lon);

            return address;
        }
        
        return Result.Failure<Address>("There is no address");

        static string FormatRequestString(Location location) =>
            $"reverse?format=jsonv2&lat={location.Lat}&lon={location.Lon}&zoom=10";
    }

    public async Task<Result<IEnumerable<Address>>> GetAddressByNameAsync(string searchString, CancellationToken c)
    {
        var response = await client.GetAsync(FormatSearchString(searchString), c);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<AddressResponse>>();

            logger.LogDebug("Got a contetnt from API: {@Content}", content);

            if (content.FirstOrDefault() is not { })
            {
                return Result.Failure<IEnumerable<Address>>("Could not parse content");
            }

            IEnumerable<Result<Address>> results = Enumerable.Empty<Result<Address>>();

            foreach(var element in content)
            {
                var address = Address.Create(element.DisplayName, element.Lat, element.Lon);
                if(address.IsSuccess) results = results.Append(address);
            }

            return Result.Combine(results, "One or more results are failed")
                .Map(() =>
                {
                    IEnumerable<Address> result = Enumerable.Empty<Address>();
                    foreach (var element in results)
                    {
                        if(element.IsSuccess) result = result.Append(element.Value);
                    }
                    return result;
                });
        }

        return Result.Failure<IEnumerable<Address>>("There is no addresses");

        static string FormatSearchString(string search) =>
            $"search?city={search}&country=россия&format=jsonv2";
    }
}
