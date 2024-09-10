using CSharpFunctionalExtensions;

namespace coal_backend.Models;

public class Address
{
    public Guid Id { get; protected init; }

    public string DisplayName { get; protected set; } = null!;

    public double Latitude { get; protected set; }

    public double Longitude { get; protected set; }

    public static Result<Address> Create(string displayName, double lat, double lon)
    {
        var result = Result.Combine(
            Result.SuccessIf(string.IsNullOrEmpty(displayName) == false, "Address should not be null or empty"),
            Result.SuccessIf((41.84 < lat)&&(lat < 77.56), "Address should be in Russia"),
            Result.SuccessIf((20.00 < lon)&&(lon < 194.85), "Address should be in Russia")
        );

        return result.Map(() =>
            new Address() { DisplayName = displayName, Latitude = lat, Longitude = lon }
        );
    }
}
