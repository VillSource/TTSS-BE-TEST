using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.POST.Zone;

public class PostEvacuationZones : Ep
    .Req<PostEvacuationZonesRequest>
    .Res<PostEvacuationZonesResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Post("evacuation-zones");
        Description(endpoint =>
        {
            endpoint.WithTags("Evacuation")
                .WithSummary("Add Evacation zones")
                .WithDescription("Adds information about evacuation zones, including location, number of people needing evacuation, and urgency level.")
                ;
        });
    }

    public override async Task HandleAsync(PostEvacuationZonesRequest req, CancellationToken ct)
    {
        Response.Msg = $"HELLO {req}";
    }
}
