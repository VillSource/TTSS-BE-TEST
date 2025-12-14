using Anirut.Evacuation.Api.Services.Database;
using Anirut.Evacuation.Api.Services.VehicleServices;
using Anirut.Evacuation.Api.Services.ZoneServices;
using StackExchange.Redis;

namespace Anirut.Evacuation.Api.Configurations;

public static class RedisConfiguration
{
    public static IServiceCollection AddRedisConfig(this IServiceCollection services, IConfigurationManager config)
    {
        string redisConnStr = config.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Connection string 'Redis' not found.");
        var redis = ConnectionMultiplexer.Connect(redisConnStr + ",allowAdmin=true");

        services.AddSingleton<IConnectionMultiplexer>(redis);
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConnectionMultiplexerFactory = async () => redis;
            options.InstanceName = "TTSS_";
        });

        return services;
    }
}
