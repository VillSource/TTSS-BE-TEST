using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.Put.Status;

public class GetEvacuationsStatus : Ep
    .Req<GetEvacuationsStatusRequest>
    .NoRes
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Get("evacuations/status");
        Description(endpoint =>
        {
            endpoint.WithTags("Evacuation")
                .WithSummary("Get Evacuation Status")
                .WithDescription("Returns the current status of all evacuation zones, including the number of\r\npeople evacuated and remaining.");
        });
    }

    public override async Task HandleAsync(GetEvacuationsStatusRequest req, CancellationToken ct)
    {
    }
}
