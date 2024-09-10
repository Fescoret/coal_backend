using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.SupplierCompanies.GettingCompanyById;

public record GetCompanyByIdCommand(Guid CompanyId);

public class GetCompanyByIdCommandHandler
{
    private readonly IDocumentSession session;

    public GetCompanyByIdCommandHandler(IDocumentSession documentSession)
    {
        this.session = documentSession;
    }

    public async Task<Result<SupplierCompany>> HandleAsync(GetCompanyByIdCommand command, CancellationToken c)
    {
        return (await session.LoadAsync<SupplierCompany>(command.CompanyId, c)).AsMaybe()
            .ToResult("Company with such ID does not exist");
    }
}
