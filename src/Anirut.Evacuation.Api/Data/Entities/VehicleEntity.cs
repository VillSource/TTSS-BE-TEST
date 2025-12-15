using Anirut.Evacuation.Api.Common.ObjectValues;

namespace Anirut.Evacuation.Api.Data.Entities;

public class VehicleEntity
{
    public string VehicleId { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Type { get; set; } = string.Empty;
    public GeoCoordinate Location { get; set; } = GeoCoordinate.Default;
    public double Speed { get; set; }

}
