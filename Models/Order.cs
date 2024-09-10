using CSharpFunctionalExtensions;

namespace KbAis.Intern.Library.Service.Web.Data.Models;

public class Order
{
    public Guid Id { get; protected init; }

    public Guid UserId { get; protected set; }

    public Guid CoalId { get; protected set; }

    public Guid Address { get; protected set; }

    public double Amount { get; protected set; }

    public long InitialDate { get; protected set; }

    public bool IsDelivered { get; protected set; }

    public Order()
    {

    }

    public static Result<Order> Create(Guid userId, Guid coalId, Guid address, double amount)
    {
        var isPropertiesCorrect = Result.Combine(
            Result.SuccessIf(userId != Guid.Empty, "UserId is requeried"),
            Result.SuccessIf(coalId != Guid.Empty, "BookId is requeried"),
            Result.SuccessIf(address != Guid.Empty, "Address is requeried"),
            Result.SuccessIf(amount > 0, "Amount shouldn't be zero or less")
        );
        return isPropertiesCorrect.Map(() =>
            new Order()
            {
                UserId = userId,
                CoalId = coalId,
                Address = address,
                Amount = amount,
                InitialDate = DateTime.Now.Ticks,
                IsDelivered = false
            }
        );
    }

    public Order MarkAsDelivered()
    {
        IsDelivered = true;
        return this;
    }
}