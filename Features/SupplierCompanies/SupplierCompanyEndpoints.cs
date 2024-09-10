using coal_backend.Features.SupplierCompanies.DeletingCompany.V1;
using coal_backend.Features.SupplierCompanies.GettingAllCompanies.V1;
using coal_backend.Features.SupplierCompanies.GettingCompanyById.V1;
using coal_backend.Features.SupplierCompanies.RegisteringCompany.V1;

namespace coal_backend.Features.SupplierCompanies;

public class SupplierCompanyEndpoints
{
    public const string ResourceName = "companies";

    public static class V1
    {
        public static void Map(WebApplication app)
        {
            RegisterCompanyEndpoint.Map(app);

            DeleteCompanyEndpoint.Map(app);

            GetAllCompaniesEndpoint.Map(app);

            GetCompanyByIdEndpoint.Map(app);
        }
    }
}
