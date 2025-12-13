namespace Anirut.Evacuation.Api.Common.ObjectValues;

public record GeoCoordinate(double Latitude, double Longitude)
{
    public GeoCoordinate() : this(0, 0) { }
    public static GeoCoordinate Default => new(0, 0);
}
