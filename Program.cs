using coal_backend.Features.Addresses;
using coal_backend.Features.Addresses.GettingAddress;
using coal_backend.Features.Addresses.GettingAddressByName;
using coal_backend.Features.Addresses.GettingSavedAddresses;
using coal_backend.Features.Addresses.SaveAddress;
using coal_backend.Features.Addresses.SaveAddress.V1;
using coal_backend.Features.CoalBrands;
using coal_backend.Features.CoalBrands.CreatingCoalBrand;
using coal_backend.Features.CoalBrands.CreatingCoalBrand.V1;
using coal_backend.Features.CoalBrands.DeletingCoalBrand;
using coal_backend.Features.CoalBrands.GettingAllCoalBrands;
using coal_backend.Features.CoalBrands.GettingCoalBrandById;
using coal_backend.Features.Orders;
using coal_backend.Features.Orders.GettingAllOrders;
using coal_backend.Features.Orders.InitiatingNewOrder;
using coal_backend.Features.Orders.InitiatingNewOrder.V1;
using coal_backend.Features.Orders.MarkAsDelivered;
using coal_backend.Features.ShortestRoutes;
using coal_backend.Features.ShortestRoutes.MakeAndSaveRoutes;
using coal_backend.Features.SupplierCompanies;
using coal_backend.Features.SupplierCompanies.DeletingCompany;
using coal_backend.Features.SupplierCompanies.GettingAllCompanies;
using coal_backend.Features.SupplierCompanies.GettingCompanyById;
using coal_backend.Features.SupplierCompanies.RegisteringCompany;
using coal_backend.Features.SupplierCompanies.RegisteringCompany.V1;
using coal_backend.Features.Users;
using coal_backend.Features.Users.DeletingUser;
using coal_backend.Features.Users.GettingAllUsers;
using coal_backend.Features.Users.LoginingUser;
using coal_backend.Features.Users.LoginingUser.V1;
using coal_backend.Features.Users.RegisteringUser;
using coal_backend.Features.Users.RegisteringUser.V1;
using coal_backend.Utils;
using Marten;
using System.Net.Http.Headers;

var MyAllowSpecificOrigins = "CoalAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});

builder.Services.AddMarten(o =>
{
    o.UseDefaultSerialization(nonPublicMembersStorage: NonPublicMembersStorage.NonPublicSetters);
    o.Connection(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IRouteFetcher, OSRMRouteFetcher>();
builder.Services.AddScoped<IAddressFetcher, NominatiumAddressFetcher>();
builder.Services.AddScoped<TokenFactory>();

builder.Services.AddHttpClient("nominatium", client => {
    const string baseAddress = "https://nominatim.openstreetmap.org/";

    client.BaseAddress = new Uri(baseAddress);

    client.DefaultRequestHeaders.Add("User-Agent", "CoalApp");

    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json; v=2.0"));
});

builder.Services.AddHttpClient("osrm", client => {
    const string baseAddress = "http://router.project-osrm.org/route/v1/driving/";

    client.BaseAddress = new Uri(baseAddress);

    client.DefaultRequestHeaders.Add("User-Agent", "CoalApp");

    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json; v=2.0"));
});

//SupplierCompany
builder.Services.AddScoped<RegisterCompanyCommandHandler>();
builder.Services.AddScoped<DeleteCompanyCommandHandler>();
builder.Services.AddScoped<GetAllCompaniesCommandHandler>();
builder.Services.AddScoped<GetCompanyByIdCommandHandler>();
//CoalBrand
builder.Services.AddScoped<CreateCoalBrandCommandHandler>();
builder.Services.AddScoped<DeleteCoalBrandCommandHandler>();
builder.Services.AddScoped<GetAllCoalBrandsCommandHandler>();
builder.Services.AddScoped<GetCoalBrandNamesCommandHandler>();
//Address
builder.Services.AddScoped<GetAddressCommandHandler>();
builder.Services.AddScoped<GetSavedAddressesCommandHandler>();
builder.Services.AddScoped<GetAddressByNameCommandHandler>();
builder.Services.AddScoped<SaveAddressCommandHandler>();
//User
builder.Services.AddScoped<RegisterUserCommandHandler>();
builder.Services.AddScoped<LoginUserCommandHandler>();
builder.Services.AddScoped<GetAllUsersCommandHandler>();
builder.Services.AddScoped<DeleteUserCommandHandler>();
//Order
builder.Services.AddScoped<InitiateNewOrderCommandHandler>();
builder.Services.AddScoped<GetAllOrdersCommandHandler>();
builder.Services.AddScoped<MarkAsDeliveredCommandHandler>();
//ShortestRoute
builder.Services.AddScoped<MakeAndSaveRoutesCommandHandler>();
//Validators
builder.Services.AddScoped<RegisterCompanyRequestValidator>();
builder.Services.AddScoped<CreateCoalBrandRequestValidator>();
builder.Services.AddScoped<SaveAddressRequestValidator>();
builder.Services.AddScoped<RegisterUserRequestValidator>();
builder.Services.AddScoped<LoginUserRequestValidator>();
builder.Services.AddScoped<LoginUserResponseValidator>();
builder.Services.AddScoped<InitiateNewOrderRequestValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

SupplierCompanyEndpoints.V1.Map(app);
CoalBrandEndpoints.V1.Map(app);
AddressEndpoints.V1.Map(app);
UserEndpoints.V1.Map(app);
OrderEndpoints.V1.Map(app);
ShortestRouteEndpoints.V1.Map(app);

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors(MyAllowSpecificOrigins);
app.Use(async (context, next) =>
{
    //context.Response.Headers.Add("X-Developed-By", "Victor");
    context.Response.Headers.Server = "Really cool server";
    await next.Invoke();
});

app.MapControllers();

app.Run();
