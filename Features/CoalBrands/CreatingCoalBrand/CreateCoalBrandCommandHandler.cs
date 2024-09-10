using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.CoalBrands.CreatingCoalBrand;

public record CreateCoalBrandCommand(Guid CompanyId, string Name, double Price, string Description);

public class CreateCoalBrandCommandHandler
{
    private readonly IDocumentSession session;

    public CreateCoalBrandCommandHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public async Task<Result> HandleAsync(CreateCoalBrandCommand command, CancellationToken c)
    {
        return await session.Load<SupplierCompany>(command.CompanyId).AsMaybe()
            .ToResult("Company with such Id does not exist")
            .Bind(company => CoalBrand.Create(command.CompanyId, command.Name, command.Price, command.Description))
            .Tap(brand => session.Store(brand))
            .Tap(() => session.SaveChangesAsync(c));
    }
}
