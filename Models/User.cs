using CSharpFunctionalExtensions;

namespace coal_backend.Models;

public class User
{
    public Guid Id { get; protected init; }

    public string FirstName { get; protected set; } = null!;

    public string? LastName { get; protected set; }

    public string Email { get; protected set; } = null!;

    public string Password { get; protected set; } = null!;

    public Guid Address { get; protected set; }

    public IEnumerable<Guid> OrderHistory { get; protected set; } = null!;

    public static Result<User> Create(string firstName, string lastName, string email, string password)
    {
        var result = Result.Combine(
            Result.SuccessIf(string.IsNullOrEmpty(firstName) == false, "FirstName should not be null or empty"),
            Result.SuccessIf(lastName is not null, "LastName should not be null"),
            Result.SuccessIf(string.IsNullOrEmpty(email) == false, "Email should not be null or empty"),
            Result.SuccessIf(string.IsNullOrEmpty(password) == false, "Password should not be null or empty")
        );

        return result.Map(() =>
            new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                Address = Guid.Empty,
                OrderHistory = Enumerable.Empty<Guid>()
            }
        );
    }

    public Result<User> ChangeEmail(string newEmail)
    {
        return Result.SuccessIf(string.IsNullOrEmpty(newEmail) == false, "Email should not be null or empty")
            .Map(() =>
            {
                Email = newEmail;
                return this;
            });
    }

    public Result<User> ChangePassword(string newPassword)
    {
        return Result.SuccessIf(string.IsNullOrEmpty(newPassword) == false, "Password should not be null or empty")
            .Map(() =>
            {
                Password = newPassword;
                return this;
            });
    }

    public Result<User> ChangeName(string firstName, string lastName)
    {
        var result = Result.Combine(
            Result.SuccessIf(string.IsNullOrEmpty(firstName) == false, "FirstName should not be null or empty"),
            Result.SuccessIf(lastName is not null, "LastName should not be null")
        );

        return result.Map(() =>
        {
            FirstName = firstName;
            LastName = lastName;
            return this;
        });
    }

    public Result<User> SaveOrder(Guid orderId, Guid address)
    {
        var result = Result.Combine(
            Result.SuccessIf(orderId != Guid.Empty, "Order id should not be empty"),
            Result.SuccessIf(address != Guid.Empty, "Address id should not be empty")
        );

        return result.Map(() =>
        {
            Address = address;
            OrderHistory.Append(orderId);
            return this;
        });
    }

    public bool Login(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException(nameof(password), "User password should not be null or empty");
        }

        if (BC.Verify(password, Password))
        {
            return true;
        }

        return false;
    }
}
