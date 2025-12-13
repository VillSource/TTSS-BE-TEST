using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.POST.Plan;

public class PostEvacuationsPlan : Ep
    .NoReq.Res<List<PostEvacuationsPlanResponse>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Version(1);
        Post("evacuations/plan");
        Description(endpoint =>
        {
            endpoint.WithTags("Evacuation")
                .WithSummary("Generate Evacation plan")
                .WithDescription("Generates a plan that assigns vehicles to evacuation zones, prioritizing areas based on urgency and vehicle capacity.") ;
        });

    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response =
        [
            new() {
                ZoneId = "zone-123",
                VecicleId = "vehicle-456",
                Eta = "15 minutes",
                NumberOfPeople = 50
            },
            new() {
                ZoneId = "zone-789",
                VecicleId = "vehicle-012",
                Eta = "30 minutes",
                NumberOfPeople = 30
            }
        ];
    }
}
