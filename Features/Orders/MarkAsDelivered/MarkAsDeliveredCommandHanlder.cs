using CSharpFunctionalExtensions;
using KbAis.Intern.Library.Service.Web.Data.Models;
using Marten;

namespace coal_backend.Features.Orders.MarkAsDelivered;

public record MarkAsDeliveredCommand(Guid OrderId);

public class MarkAsDeliveredCommandHandler
{
    private readonly IDocumentSession session;

    public MarkAsDeliveredCommandHandler(IDocumentSession documentSession)
    {
        session = documentSession;
    }

    public async Task<Result> HandleAsync(MarkAsDeliveredCommand command, CancellationToken c)
    {
        return await session.Load<Order>(command.OrderId).AsMaybe()
            .ToResult("Order with such id not found")
            .Bind(order => Result.Success(order.MarkAsDelivered()))
            .Tap(order => session.Update(order))
            .Tap(() => session.SaveChangesAsync(c));
    }
}
