using System.Text.Json.Serialization;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.POST.Plan;

public class PostEvacuationsPlanResponse
{
    public string ZoneId { get; set; } = string.Empty;
    public string VecicleId { get; set; } = string.Empty;
    [JsonPropertyName("ETA")]
    public string Eta { get; set; } = string.Empty;
    public int NumberOfPeople { get; set; }

}
