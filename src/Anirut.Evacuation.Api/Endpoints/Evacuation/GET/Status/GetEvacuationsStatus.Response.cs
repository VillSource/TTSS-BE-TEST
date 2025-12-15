namespace Anirut.Evacuation.Api.Endpoints.Evacuation.Put.Status;

public class GetEvacuationsStatusResponse
{
    public string ZoneId { get; set; } = string.Empty;
    public int TotalEvacuated { get; set; }
    public int RemainingPeople { get; set; }
    public string LastVehiclesUse { get; set; } = string.Empty;
}
