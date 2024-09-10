using coal_backend.Features.ShortestRoutes.MakeAndSaveRoutes.V1;

namespace coal_backend.Features.ShortestRoutes;

public class ShortestRouteEndpoints
{
    public const string ResourceName = "routes";

    public static class V1
    {
        public static void Map(WebApplication app)
        {
            MakeAndSaveRoutesEndpoint.Map(app);
        }
    }
}
