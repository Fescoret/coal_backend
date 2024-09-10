using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;
using OsmSharp;

namespace coal_backend.Features.SupplierCompanies.RegisteringCompany;

public record RegisterCompanyCommand(string CompanyName, string Description, Guid Location);

public class RegisterCompanyCommandHandler
{
    private readonly IDocumentSession session;

    public RegisterCompanyCommandHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public async Task<Result> HandleAsync(RegisterCompanyCommand command, CancellationToken c)
    {
        return await SupplierCompany.Create(command.CompanyName, command.Description, command.Location)
                .Tap(author => session.Store(author))
                .Tap(() => session.SaveChangesAsync(c));
    }
}
