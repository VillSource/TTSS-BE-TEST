using Anirut.Evacuation.Api.Services.ZoneServices;
using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.POST.Zone;

public class PostEvacuationZones : Ep
    .Req<List<PostEvacuationZonesRequest>>
    .NoRes
{
    private readonly IZoneService _service;

    public PostEvacuationZones(IZoneService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Post("evacuation-zones");
        Description(endpoint =>
        {
            endpoint.WithTags("Evacuation")
                .WithSummary("Add Evocation zones")
                .WithDescription("Adds information about evacuation zones, including location, number of people needing evacuation, and urgency level.")
                ;
        });
    }

    public override async Task HandleAsync(List<PostEvacuationZonesRequest> req, CancellationToken ct)
    {
        await _service.AddRange(req.Select(r => r.ToEntity()), ct);
        await Send.NoContentAsync(ct);
    }
}
