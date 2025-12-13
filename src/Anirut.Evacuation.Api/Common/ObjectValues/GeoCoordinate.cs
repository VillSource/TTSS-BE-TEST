namespace Anirut.Evacuation.Api.Common.ObjectValues;

public readonly record struct GeoCoordinate(double Latitude, double Longitude)
{
    public GeoCoordinate() : this(0, 0) { }
    public static GeoCoordinate Default => new(0, 0);
}
