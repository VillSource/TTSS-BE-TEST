using Anirut.Evacuation.Api.Services.EvacuationStatusServices;
using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.Put.Status;

public class GetEvacuationsStatus : Ep
    .NoReq
    .Res<List<GetEvacuationsStatusResponse>>
{
    private readonly EvacuationStatusServices _evacuationStatusServices;

    public GetEvacuationsStatus(EvacuationStatusServices evacuationStatusServices)
    {
        _evacuationStatusServices = evacuationStatusServices;
    }

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

    public override async Task HandleAsync(CancellationToken ct)
    {
        var status = await _evacuationStatusServices.GetStatus();
        Response = status.Select(s => new GetEvacuationsStatusResponse
        {
            LastVehiclesUse = s.LastVehiclesUse,
            RemainingPeople = s.RemainingPeople,
            TotalEvacuated = s.TotalEvacuated,
            ZoneId = s.ZoneId
        }).ToList();
    }
}
