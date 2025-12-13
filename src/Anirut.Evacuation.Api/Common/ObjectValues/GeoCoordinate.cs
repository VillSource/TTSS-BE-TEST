
namespace Anirut.Evacuation.Api.Common.ObjectValue;

public readonly record struct GeoCoordinate(double Latitude, double Longitude)
{
    public static GeoCoordinate Default => new(0, 0);
}
