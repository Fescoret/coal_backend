using CSharpFunctionalExtensions;
using OsmSharp;

namespace coal_backend.Models;

public class SupplierCompany
{
    public Guid Id { get; protected init; }

    public string CompanyName { get; protected set; } = null!;

    public string Description { get; protected set; } = null!;

    public Guid Location { get; protected set; }

    public static Result<SupplierCompany> Create(string companyName, string description, Guid location)
    {
        var result = Result.Combine(
            Result.SuccessIf(string.IsNullOrEmpty(companyName) == false, "Company name should not be null or empty"),
            Result.SuccessIf(string.IsNullOrEmpty(description) == false, "Company description should not be null or empty"),
            Result.SuccessIf(location != Guid.Empty, "Company should have its location to calculate order cost")
        );

        return result.Map(() => 
            new SupplierCompany() { CompanyName = companyName, Description = description, Location = location }
        );
    }

    public Result<SupplierCompany> ChangeName(string newName)
    {
        return Result.SuccessIf(string.IsNullOrEmpty(newName) == false, "Company name should not be null or empty")
            .Map(() =>
            {
                CompanyName = newName;
                return this;
            });
    }

    public Result<SupplierCompany> ChangeDescription(string newDescription)
    {
        return Result.SuccessIf(string.IsNullOrEmpty(newDescription) == false, "Company description should not be null or empty")
            .Map(() =>
            {
                Description = newDescription;
                return this;
            });
    }

    public Result<SupplierCompany> ChangeLocation(Guid newLocation)
    {
        return Result.SuccessIf(newLocation != Guid.Empty, "Company location should not be null or empty")
            .Map(() =>
            {
                Location = newLocation;
                return this;
            });
    }
}
