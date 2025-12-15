
namespace Anirut.Evacuation.Api.Common.ObjectValues;

public record GeoCoordinate(double Latitude, double Longitude)
{
    public GeoCoordinate() : this(0, 0) { }
    public static GeoCoordinate Default => new(0, 0);
    public static readonly double EarthRadiusKm = 6371.04;

    public static double ToRadians(double distance) => distance * (Math.PI / 180);
    public double HaversineDistanceTo(GeoCoordinate other)
    {
        /****************************************************************
        *    Apply Formula:                                             *
        *    a = sin²(φB — φA / 2) +cos φA* cos φB* sin²(λB — λA / 2)   *
        *    c = 2 * atan2( √a, √(1−a) )                                *
        *    d = R ⋅ c                                                  *
        *                                                               *
        *    φ = latitude in radians                                    *
        *    λ = longitude in radians                                   *
        *    R = earth's average radius                                 *
        *        3958.8 is earth's average radius in miles.             *
        *        6371.04 is earth's average radius in kilometers.       *
        ****************************************************************/

        double dLat = ToRadians(other.Latitude - Latitude);
        double dLon = ToRadians(other.Longitude - Longitude);

        double lat1 = ToRadians(Latitude);
        double lat2 = ToRadians(other.Latitude);
        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                   Math.Pow(Math.Sin(dLon / 2), 2) *
                   Math.Cos(lat1) * Math.Cos(lat2);
        double c = 2 * Math.Asin(Math.Sqrt(a));
        return EarthRadiusKm * c;
    }
}
