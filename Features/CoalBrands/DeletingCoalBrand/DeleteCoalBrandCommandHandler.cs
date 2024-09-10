using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.CoalBrands.DeletingCoalBrand;

public record DeleteCoalBrandCommand(Guid BrandId);

public class DeleteCoalBrandCommandHandler
{
    private readonly IDocumentSession session;

    public DeleteCoalBrandCommandHandler(IDocumentSession documentSession)
    {
        this.session = documentSession;
    }

    public async Task<Result> HandleAsync(DeleteCoalBrandCommand command, CancellationToken c)
    {
        return await session.Load<CoalBrand>(command.BrandId).AsMaybe()
            .ToResult("Brand with such ID does not exist")
            .Tap(brand => session.Delete(brand))
            .Tap(() => session.SaveChangesAsync(c));
    }
}
