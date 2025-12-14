using Anirut.Evacuation.Api.Common.Enumns;
using Anirut.Evacuation.Api.Common.ObjectValues;
using Anirut.Evacuation.Api.Data.Entities;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.POST.Zone;

public class PostEvacuationZonesRequest
{
    public string ZoneId { get; set; } = string.Empty;
    public GeoCoordinate LocationCoordinates { get; set; } = GeoCoordinate.Default;
    public int NumberOfPeople { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }

    internal ZoneEntity ToEntity() => new()
    {
        ZoneId = ZoneId,
        LocationCoordinates = LocationCoordinates,
        NumberOfPeople = NumberOfPeople,
        UrgencyLevel = UrgencyLevel
    };
}
