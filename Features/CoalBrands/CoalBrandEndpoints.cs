using coal_backend.Features.CoalBrands.CreatingCoalBrand.V1;
using coal_backend.Features.CoalBrands.DeletingCoalBrand.V1;
using coal_backend.Features.CoalBrands.GettingAllCoalBrands.V1;
using coal_backend.Features.CoalBrands.GettingCoalBrandById.V1;

namespace coal_backend.Features.CoalBrands;

public class CoalBrandEndpoints
{
    public const string ResourceName = "brands";

    public static class V1
    {
        public static void Map(WebApplication app)
        {
            CreateCoalBrandEndpoint.Map(app);

            DeleteCoalBrandEndpoint.Map(app);

            GetAllCoalBrandsEndpoint.Map(app);

            GetCoalBrandNamesEndpoint.Map(app);
        }
    }
}
