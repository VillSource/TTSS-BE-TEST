namespace Anirut.Evacuation.Api.Services.PlanServices.Models;

public class PlanResult
{
    public string ZoneId { get; set; } = string.Empty;
    public string VecicleId { get; set; } = string.Empty;
    public string Eta { get; set; } = string.Empty;
    public int NumberOfPeople { get; set; }

}
