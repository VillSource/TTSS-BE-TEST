using Anirut.Evacuation.Api.Services.EvacuationStatusServices;
using FastEndpoints;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.Put.Status;

public class PutEvacuationsStatus : Ep
    .Req<List<PutEvacuationsStatusRequest>>
    .NoRes
{
    private readonly EvacuationStatusServices _service;

    public PutEvacuationsStatus(EvacuationStatusServices service)
    {
        _service = service;
    }

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

    public override async Task HandleAsync(List<PutEvacuationsStatusRequest> req, CancellationToken ct)
    {
        var x = req.Select(r => new B
        {
            ZoneId = r.ZoneId,
            TotalMove = r.TotalMove,
            VehicleId = r.VehicleId
        }).ToList();

        await _service.UpdateStatus(x);
    }
}
