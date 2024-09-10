using CSharpFunctionalExtensions;

namespace coal_backend.Models;

public class CoalBrand
{
    public Guid Id { get; protected init; }

    public Guid CompanyId { get; protected set; }

    public string Name { get; protected set; } = null!;

    public double Price { get; protected set; } // in rubles for kilogramm

    public string Description { get; protected set; } = null!;

    public static Result<CoalBrand> Create(Guid companyId, string brandName, double price, string description)
    {
        var result = Result.Combine(
            Result.SuccessIf(companyId != Guid.Empty, "Company id should not be empty"),
            Result.SuccessIf(string.IsNullOrEmpty(brandName) == false, "Brand name should not be null or empty"),
            Result.SuccessIf(price > 0, "Price should not be non pozitive"),
            Result.SuccessIf(string.IsNullOrEmpty(description) == false, "Description should not be null or empty")
        );

        return result.Map(() => 
            new CoalBrand() { CompanyId = companyId, Name = brandName, Price = price, Description = description }
        );
    }

    public Result<CoalBrand> ChangeName(string newName)
    {
        return Result.SuccessIf(string.IsNullOrEmpty(newName) == false, "Brand name should not be null or empty")
            .Map(() =>
            {
                Name = newName;
                return this;
            });
    }

    public Result<CoalBrand> ChangePrice(double newPrice)
    {
        return Result.SuccessIf(newPrice > 0, "Price should not be non pozitive")
            .Map(() =>
            {
                Price = newPrice;
                return this;
            });
    }

    public Result<CoalBrand> ChangeDescription(string newDescription)
    {
        return Result.SuccessIf(string.IsNullOrEmpty(newDescription) == false, "Description should not be null or empty")
            .Map(() =>
            {
                Description = newDescription;
                return this;
            });
    }
}
