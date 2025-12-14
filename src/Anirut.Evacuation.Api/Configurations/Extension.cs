
namespace Anirut.Evacuation.Api.Configurations;

public static class Extension
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfigurationManager config)
    {
        services
            .AddServices()
            .AddRedisConfig(config)
            .AddDbConfig(config)
            .AddCorsConfig()
            .AddOpenApiConfig();

        return services;
    }
}
