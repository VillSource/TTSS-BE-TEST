using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.Put.Status;

public class PutEvacuationsStatus : Ep
    .Req<PutEvacuationsStatusRequest>
    .NoRes
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Put("evacuations/status");
        Description(endpoint =>
        {
            endpoint.WithTags("Evacuation")
                .WithSummary("Update Evacuation Status")
                .WithDescription("Updates the evacuation status by specifying the number of evacuees moved from an area and the vehicle used for the operation.");
        });
    }

    public override async Task HandleAsync(PutEvacuationsStatusRequest req, CancellationToken ct)
    {
    }
}
