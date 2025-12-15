using Anirut.Evacuation.Api.Services.PlanServices;
using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.POST.Plan;

public class PostEvacuationsPlan : Ep
    .NoReq.Res<List<PostEvacuationsPlanResponse>>
{
    private readonly PlanService _planService;

    public PostEvacuationsPlan(PlanService planService)
    {
        _planService = planService;
    }

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
        var res = _planService.CalculatePlan();

        Response = [.. res.Select( r => new PostEvacuationsPlanResponse
        {
            NumberOfPeople = r.NumberOfPeople,
            VecicleId = r.VecicleId,
            ZoneId = r.ZoneId,
            Eta = ""
        })];
    }
}
