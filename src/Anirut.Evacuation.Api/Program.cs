using Anirut.Evacuation.Api.Data;
using Anirut.Evacuation.Api.Services.Database;
using Anirut.Evacuation.Api.Services.VehicleServices;
using Anirut.Evacuation.Api.Services.ZoneServices;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IZoneService, ZoneService>();

string redisConnStr = builder.Configuration.GetConnectionString("Redis")
    ?? throw new InvalidOperationException("Connection string 'Redis' not found.");
var redis = ConnectionMultiplexer.Connect(redisConnStr + ",allowAdmin=true");
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConnectionMultiplexerFactory = async () => redis;
    options.InstanceName = "TTSS_"; 
});


builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Npgsql")
                           ?? throw new InvalidOperationException("Connection string 'Npgsql' not found.");
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
app.UseHttpsRedirection();

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
