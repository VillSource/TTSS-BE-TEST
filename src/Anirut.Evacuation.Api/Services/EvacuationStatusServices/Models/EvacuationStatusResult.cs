namespace Anirut.Evacuation.Api.Services.EvacuationStatusServices.Models;

public class EvacuationStatusResult
{
    public string ZoneId { get; set; } = string.Empty;
    public int TotalEvacuated { get; set; }
    public int RemainingPeople { get; set; }
    public string LastVehiclesUse { get; set; } = string.Empty;

}
