using Anirut.Evacuation.Api.Common.ObjectValues;
using Anirut.Evacuation.Api.Data.Entities;

namespace Anirut.Evacuation.Api.Endpoints.Vehicles.Post;

public class PostVehiclesRequest
{
    public string VehicleId { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Type { get; set; } = string.Empty;
    public GeoCoordinate LocationCoordinates { get; set; } = GeoCoordinate.Default;
    public double Speed { get; set; }

    internal VehicleEntity ToEntity() => new()
    {
        VehicleId = VehicleId,
        Capacity = Capacity,
        Type = Type,
        Location = LocationCoordinates,
        Speed = Speed
    };
}
