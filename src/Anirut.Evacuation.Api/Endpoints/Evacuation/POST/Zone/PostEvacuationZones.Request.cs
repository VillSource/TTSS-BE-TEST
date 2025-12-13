using Anirut.Evacuation.Api.Common.Enumns;
using Anirut.Evacuation.Api.Common.ObjectValue;

namespace Anirut.Evacuation.Api.Endpoints.Evacuation.POST.Zone;

public class PostEvacuationZonesRequest
{
    public Guid ZoneId { get; set; } = Guid.NewGuid();
    public GeoCoordinate Locatioin { get; set; } = GeoCoordinate.Default;
    public int NumberOfPeople { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }
}
