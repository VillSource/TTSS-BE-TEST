namespace Anirut.Evacuation.Api.Services.PlanServices;

public partial class PlanService
{
    public class RoutingSolution
    {
        public string VehicleId { get; set; } = string.Empty;
        public List<string> ZoneIdsVisited { get; set; } = [];
        public double TotalTimeSeconds { get; set; }
    }
}
