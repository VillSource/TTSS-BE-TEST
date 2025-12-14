using Anirut.Evacuation.Api.Services.VehicleServices;
using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Vehicles.Post;

public class PostVehicles : Ep
    .Req<List<PostVehiclesRequest>>
    .NoRes
{
    private readonly IVehicleService _service;

    public PostVehicles(IVehicleService service)
    {
        _service = service;
    }

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
    public override async Task HandleAsync(List<PostVehiclesRequest> req, CancellationToken ct= default)
    {
        await _service.AddRange(req.Select(r => r.ToEntity()), ct);
        var x = _service.GetAll(ct);
    }
}
