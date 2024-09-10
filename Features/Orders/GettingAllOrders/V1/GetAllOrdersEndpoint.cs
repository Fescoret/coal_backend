using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace coal_backend.Features.Orders.GettingAllOrders.V1;

public class GetAllOrdersEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/orders", HandleRequestAsync)
            .WithTags("orders");
    }

    private static async Task<WebResult> HandleRequestAsync(
        [FromServices] GetAllOrdersCommandHandler commandHandler,
        CancellationToken c
    )
    {
        var result = await commandHandler.HandleAsync(c)
            .MapError(error => Results.BadRequest(error))
            .Map(orders => Results.Ok(orders));

        return result.IsSuccess ? result.Value : result.Error;
    }
}
