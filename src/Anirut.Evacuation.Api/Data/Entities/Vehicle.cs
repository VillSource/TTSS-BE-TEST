using Anirut.Evacuation.Api.Common.ObjectValues;

namespace Anirut.Evacuation.Api.Data.Entities;

public class Vehicle
{
    public string VehicleId { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Type { get; set; } = string.Empty;
    public GeoCoordinate LocationCoordinates { get; set; }
    public double Speed { get; set; }

}
