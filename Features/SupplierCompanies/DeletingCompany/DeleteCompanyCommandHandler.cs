using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.SupplierCompanies.DeletingCompany;

public record DeleteCompanyCommand(Guid CompanyId);

public class DeleteCompanyCommandHandler
{
    private readonly IDocumentSession session;

    public DeleteCompanyCommandHandler(IDocumentSession documentSession)
    {
        this.session = documentSession;
    }

    public async Task<Result> HandleAsync(DeleteCompanyCommand command, CancellationToken c)
    {
        return await session.Load<SupplierCompany>(command.CompanyId).AsMaybe()
            .ToResult("Company with such ID does not exist")
            .Tap(company => session.Delete(company))
            .Tap(() => session.SaveChangesAsync(c));
    }
}
