using coal_backend.Models;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using KbAis.Intern.Library.Service.Web.Data.Models;
using Marten;

namespace coal_backend.Features.Orders.InitiatingNewOrder;

public record InitiateNewOrderCommand(string UserEmail, string Address, Guid CoalId, double Amount);

public class InitiateNewOrderCommandHandler
{
    private readonly IDocumentSession session;

    public InitiateNewOrderCommandHandler(IDocumentSession documentSession)
    {
        this.session = documentSession;
    }

    private static Result<Order> TryToOrder(IDocumentSession session, Order order, Guid UserId)
    {
        return session.Load<User>(UserId).AsMaybe()
            .ToResult("User not found (maybe it is deleted)")
            .Bind(user => user.SaveOrder(order.Id, order.Address))
            .Tap(user => session.Update(user))
            .Tap(() => session.Store(order))
            .Map(orders => { return order; });
    }

    private static Result<Guid> UserIdByEmail(IDocumentSession session, string email)
    {
        return session.Query<User>()
            .Where(x => x.Email == email)
            .FirstOrDefault().AsMaybe()
            .ToResult("User not found (maybe it is deleted)")
            .Bind(user => Result.Success(user.Id));
    }

    public async Task<Result> HandleAsync(InitiateNewOrderCommand command, CancellationToken c)
    {
        return await session.Query<Address>()
            .Where(x => x.DisplayName == command.Address)
            .FirstOrDefault()
            .AsMaybe().ToResult("There is no address with this name")
            .Bind(address => 
            {
                var UserId = UserIdByEmail(session, command.UserEmail);
                if (UserId.IsSuccess)
                {
                    return Order.Create(UserId.Value, address.Id, command.CoalId, command.Amount);
                }
                return Result.Failure<Order>("There is no user with this email");
            })
            .Bind(order => TryToOrder(session, order, order.UserId))
            .Tap(() => session.SaveChangesAsync(c));
    }
}
