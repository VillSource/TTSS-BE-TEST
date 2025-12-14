using Anirut.Evacuation.Api.Services.Database;
using Anirut.Evacuation.Api.Services.VehicleServices;
using Anirut.Evacuation.Api.Services.ZoneServices;

namespace Anirut.Evacuation.Api.Configurations;

public static class CorsConfigration
{
    public static IServiceCollection AddCorsConfig(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            const string DefaultCorsPolicy = "DefaultCorsPolicy";
            options.AddPolicy(DefaultCorsPolicy, policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}
