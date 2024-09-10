using coal_backend.Models;
using CSharpFunctionalExtensions;
using Marten;

namespace coal_backend.Features.SupplierCompanies.GettingAllCompanies;

public class GetAllCompaniesCommandHandler
{
    private readonly IQuerySession session;

    public GetAllCompaniesCommandHandler(IQuerySession documentSession)
    {
        this.session = documentSession;
    }

    public async Task<Result<IReadOnlyList<SupplierCompany>>> HandleAsync(CancellationToken c)
    {
        return (await session.Query<SupplierCompany>().ToListAsync(c)).AsMaybe()
            .ToResult("Server Error: Companies table does'nt exist")
            .Bind(companies => companies.IsEmpty()
                        ? Result.Failure<IReadOnlyList<SupplierCompany>>("There is no companies")
                        : Result.Success(companies));
    }
}
