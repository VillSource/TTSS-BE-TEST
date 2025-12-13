using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.POST.Plan;

public class PostEvacuationsPlan : Ep
    .NoReq.NoRes
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Post("evacuations/plan");
        Description(endpoint =>
        {
            endpoint.WithTags("Evacuation")
                .WithSummary("Add Evacation plan")
                .WithDescription("Generates a plan that assigns vehicles to evacuation zones, prioritizing areas based on urgency and vehicle capacity.") ;
        });

    }

    public override Task HandleAsync(CancellationToken ct)
    {
        return base.HandleAsync(ct);
    }
}
