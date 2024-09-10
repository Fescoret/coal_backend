using Baseline;
using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.CoalBrands.GettingAllCoalBrands;

public class GetAllCoalBrandsCommandHandler
{
    private readonly IQuerySession session;

    public GetAllCoalBrandsCommandHandler(IQuerySession documentSession)
    {
        this.session = documentSession;
    }

    public async Task<Result<IReadOnlyList<CoalBrand>>> HandleAsync(CancellationToken c)
    {
        return (await session.Query<CoalBrand>().ToListAsync(c)).AsMaybe()
            .ToResult("Server Error: Coal brands table does'nt exist")
            .Bind(brands => brands.IsEmpty()
                        ? Result.Failure<IReadOnlyList<CoalBrand>>("There is no coal brands")
                        : Result.Success(brands));
    }
}
