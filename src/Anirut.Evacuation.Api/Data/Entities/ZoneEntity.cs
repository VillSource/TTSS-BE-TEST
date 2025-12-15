using Anirut.Evacuation.Api.Common.Enumns;
using Anirut.Evacuation.Api.Common.ObjectValues;

namespace Anirut.Evacuation.Api.Data.Entities;

public class ZoneEntity
{
    public string ZoneId { get; set; } = string.Empty;
    public GeoCoordinate Location { get; set; } = GeoCoordinate.Default;
    public int NumberOfPeople { get; set; }
    public int TotalPeople { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }
    public string? LastVehicleId { get; set; }
}
