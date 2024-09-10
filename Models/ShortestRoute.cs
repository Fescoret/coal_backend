using Baseline;
using coal_backend.Utils;
using CSharpFunctionalExtensions;

namespace coal_backend.Models;

public class ShortestRoute
{
    public Guid Id { get; protected init; }

    public double Distance { get; protected set; }

    public string From { get; protected set; } = null!;

    public string To { get; protected set; } = null!;

    public static Result<ShortestRoute> Create(double distance, string from, string to)
    {
        var result = Result.Combine(
            Result.SuccessIf(distance > 0, "Distance should be positive"),
            Result.SuccessIf(from.IsNotEmpty(), "Company address name can't be nul or empty"),
            Result.SuccessIf(to.IsNotEmpty(), "User address name can't be nul or empty")
        );

        return result.Map(() =>
            new ShortestRoute() { Distance = distance, From = from, To = to }
        );
    }
}
