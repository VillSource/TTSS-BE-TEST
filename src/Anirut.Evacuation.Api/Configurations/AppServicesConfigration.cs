using Anirut.Evacuation.Api.Services.Database;
using Anirut.Evacuation.Api.Services.VehicleServices;
using Anirut.Evacuation.Api.Services.ZoneServices;

namespace Anirut.Evacuation.Api.Configurations;

public static class AppServicesConfigration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IDatabaseService, DatabaseService>();
        services.AddScoped<IZoneService, ZoneService>();

        return services;
    }
}
