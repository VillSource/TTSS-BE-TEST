using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Vehicles.Post;

public class PostVehicles : Ep
    .Req<List<PostVehiclesRequest>>
    .NoRes
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Post("vehicles");
        Description(endpoint =>
        {
            endpoint.WithTags("Vehicles")
                .WithSummary("Add Vehicles")
                .WithDescription("Adds information about available vehicles, such as capacity, type, and location.");
        });
    }
    public override async Task HandleAsync(List<PostVehiclesRequest> req, CancellationToken ct)
    {
    }
}
