namespace Anirut.Evacuation.Api.Endpoints.Evacuation.Put.Status;

public class PutEvacuationsStatusRequest
{
    public string VehicleId { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty;
    public int TotalMove { get; set; }
}
