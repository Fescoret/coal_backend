using coal_backend.Features.Orders.GettingAllOrders.V1;
using coal_backend.Features.Orders.InitiatingNewOrder.V1;
using coal_backend.Features.Orders.MarkAsDelivered.V1;

namespace coal_backend.Features.Orders;

public static class OrderEndpoints
{
    public const string ResourceName = "orders";

    public static class V1
    {
        public static void Map(WebApplication app)
        {
            InitiateNewOrderEndpoint.Map(app);

            GetAllOrdersEndpoint.Map(app);

            MarkAsDeliveredEndpoint.Map(app);
        }
    }
}
