using Anirut.Evacuation.Api.Services.Database;
using Anirut.Evacuation.Api.Services.VehicleServices;
using Anirut.Evacuation.Api.Services.ZoneServices;
using Microsoft.OpenApi;

namespace Anirut.Evacuation.Api.Configurations;

public static class OpenApiConfiguration
{
    public static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Servers = [];
                return Task.CompletedTask;
            });
        });
        return services;
    }
}
