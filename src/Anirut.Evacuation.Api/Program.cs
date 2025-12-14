using Anirut.Evacuation.Api.Data;
using Anirut.Evacuation.Api.Services.VehicleServices;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("A")
                           ?? throw new InvalidOperationException("Connection string 'EvacuationDatabase' not found.");
    options.UseNpgsql(connectionString);
});

builder.Services.AddCors(options =>
{
    const string DefaultCorsPolicy = "DefaultCorsPolicy";
    options.AddPolicy(DefaultCorsPolicy, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();

var app = builder.Build();

// --- Database connectivity check ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("Startup.DatabaseCheck");

    try
    {
        var db = services.GetRequiredService<DataContext>();
        var canConnect = await db.Database.CanConnectAsync();
        if (!canConnect)
        {
            logger.LogCritical("Database is not connectable. Aborting startup.");
            throw new InvalidOperationException("Unable to connect to the database.");
        }

        db.Database.EnsureCreated();

        logger.LogInformation("Database connection successful.");
    }
    catch (Exception ex)
    {
        loggerFactory.CreateLogger("Startup.DatabaseCheck").LogCritical(ex, "Database connectivity check failed.");
        throw;
    }
}
// --- End database check ---

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", () => Results.Redirect("scalar")).ExcludeFromDescription();

app.UseHttpsRedirection();

app.UseCors("DefaultCorsPolicy");

app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
    config.Versioning.Prefix = "v";
    config.Versioning.PrependToRoute = true;
});


app.Run();
