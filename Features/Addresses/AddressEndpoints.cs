using coal_backend.Features.Addresses.GettingAddress.V1;
using coal_backend.Features.Addresses.GettingAddressByName.V1;
using coal_backend.Features.Addresses.GettingSavedAddresses.V1;
using coal_backend.Features.Addresses.SaveAddress.V1;

namespace coal_backend.Features.Addresses;

public class AddressEndpoints
{
    public const string ResourceName = "addresses";

    public static class V1
    {
        public static void Map(WebApplication app)
        {
            GetAddressEndpoint.Map(app);

            GetSavedAddressesEndpoint.Map(app);

            GetAddressByNameEndpoint.Map(app);

            SaveAddressEndpoint.Map(app);
        }
    }
}
