using Anirut.Evacuation.Api.Common.Enumns;
using Anirut.Evacuation.Api.Common.ObjectValues;

namespace Anirut.Evacuation.Api.Data.Entities;

public class ZoneEntity
{
    public string ZoneId { get; set; } = string.Empty;
    public GeoCoordinate LocationCoordinates { get; set; } = GeoCoordinate.Default;
    public int NumberOfPeople { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }
}
