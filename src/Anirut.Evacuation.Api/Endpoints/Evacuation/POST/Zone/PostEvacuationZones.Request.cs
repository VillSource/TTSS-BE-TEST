using Anirut.Evacuation.Api.Common.Enumns;
using Anirut.Evacuation.Api.Common.ObjectValues;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.POST.Zone;

public class PostEvacuationZonesRequest
{
    public string ZoneId { get; set; } = string.Empty;
    public GeoCoordinate LocatioinCordinates { get; set; } = GeoCoordinate.Default;
    public int NumberOfPeople { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }
}
