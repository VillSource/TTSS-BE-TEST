using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.Delete.Clear;

public class DeleteEvacuationsClear : Ep.NoReq.NoRes
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Delete("evacuations/clear");
        Description(endpoint =>
        {
            endpoint.WithTags("Evacuation")
                .WithSummary("Clear All Data")
                .WithDescription("Clears all current evacuation plans and resets the data (useful for\r\nrestarting operations after a completed evacuation).");
        });
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        return base.HandleAsync(ct);
    }
}
