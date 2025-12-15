namespace Anirut.Evacuation.Api.Services.EvacuationStatusServices.Models;

public class EvacuationStatusUpdate
{
    public string VehicleId { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty;
    public int TotalMove { get; set; }
}
