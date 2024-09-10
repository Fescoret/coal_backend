using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Orders.MarkAsDelivered.V1;

public class MarkAsDeliveredEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPut($"/orders/{{order-id}}/issue", HandleRequestAsync)
            .WithTags("orders");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromRoute(Name = "order-id")] Guid orderId,
        [FromServices] MarkAsDeliveredCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await commandHandler.HandleAsync(new(orderId), c)
            .MapError(error => Results.BadRequest(error))
            .Map(() => Results.Ok("Order successfully delivered"));

        return result.IsSuccess ? result.Value : result.Error;
    }
}