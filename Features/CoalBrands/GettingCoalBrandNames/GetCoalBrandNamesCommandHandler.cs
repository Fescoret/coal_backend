using Baseline;
using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.CoalBrands.GettingCoalBrandById;

public class GetCoalBrandNamesCommandHandler
{
    private readonly IDocumentSession session;

    public GetCoalBrandNamesCommandHandler(IDocumentSession documentSession)
    {
        this.session = documentSession;
    }

    public List<string> BrandsNames(IReadOnlyList<CoalBrand> brands)
    {
        List<string> result = new List<string>();
        brands.Each(b => {
            if (!result.Contains(b.Name))
            {
                result.Add(b.Name);
            }
        });
        return result;
    }

    public async Task<Result<List<string>>> HandleAsync(CancellationToken c)
    {
        return (await session.Query<CoalBrand>()
            .ToListAsync(c))
            .AsMaybe()
            .ToResult("Coal brands table does not exist")
            .Bind(brands => brands.Any()
                ? Result.Success(BrandsNames(brands))
                : Result.Failure<List<string>>("Coal brands table is empty"));
    }
}
