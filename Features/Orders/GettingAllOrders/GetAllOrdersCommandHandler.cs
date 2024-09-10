using CSharpFunctionalExtensions;
using KbAis.Intern.Library.Service.Web.Data.Models;
using Marten;

namespace coal_backend.Features.Orders.GettingAllOrders;

public class GetAllOrdersCommandHandler
{
    private readonly IDocumentSession session;

    public GetAllOrdersCommandHandler(IDocumentSession documentSession)
    {
        this.session = documentSession;
    }

    public async Task<Result<List<Order>>> HandleAsync(CancellationToken c)
    {
        return await session.Query<Order>()
                .Where(x => !x.IsDelivered) // only active orders
                .ToList()
            .AsMaybe()
            .ToResult("Server Error: Orders table does'nt exist")
            .Bind(orders => orders.AnyTenant()
                ? Result.Failure<List<Order>>("There is no active orders")
                : Result.Success(orders))
            .Tap(() => session.SaveChangesAsync(c));
    }
}
