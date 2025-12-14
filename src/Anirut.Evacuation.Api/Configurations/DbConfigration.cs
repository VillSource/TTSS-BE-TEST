using Anirut.Evacuation.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Anirut.Evacuation.Api.Configurations;

public static class DbConfiguration
{
    public static IServiceCollection AddDbConfig(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddDbContext<DataContext>(options =>
        {
            var connectionString = config.GetConnectionString("Npgsql")
                                   ?? throw new InvalidOperationException("Connection string 'Npgsql' not found.");
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static bool CreateDatabase(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Database.EnsureCreated();
        return true;
    }
}
