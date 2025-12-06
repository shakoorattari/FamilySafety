namespace FamilySafety.Domain.ValueObjects;

/// <summary>
/// Value object representing geographical coordinates
/// </summary>
public record GeoLocation
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? Altitude { get; init; }
    public double? Accuracy { get; init; }

    private GeoLocation(double latitude, double longitude, double? altitude = null, double? accuracy = null)
    {
        Latitude = latitude;
        Longitude = longitude;
        Altitude = altitude;
        Accuracy = accuracy;
    }

    public static GeoLocation Create(double latitude, double longitude, double? altitude = null, double? accuracy = null)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90");
        
        if (longitude < -180 || longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180");

        return new GeoLocation(latitude, longitude, altitude, accuracy);
    }

    /// <summary>
    /// Calculate distance to another location in meters using Haversine formula
    /// </summary>
    public double DistanceTo(GeoLocation other)
    {
        const double EarthRadiusMeters = 6371000;

        var lat1Rad = ToRadians(Latitude);
        var lat2Rad = ToRadians(other.Latitude);
        var deltaLatRad = ToRadians(other.Latitude - Latitude);
        var deltaLonRad = ToRadians(other.Longitude - Longitude);

        var a = Math.Sin(deltaLatRad / 2) * Math.Sin(deltaLatRad / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(deltaLonRad / 2) * Math.Sin(deltaLonRad / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusMeters * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}

/// <summary>
/// Value object representing an address
/// </summary>
public record Address
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;

    public static Address Create(string street, string city, string state, string country, string postalCode)
    {
        return new Address
        {
            Street = street,
            City = city,
            State = state,
            Country = country,
            PostalCode = postalCode
        };
    }

    public override string ToString()
    {
        return $"{Street}, {City}, {State} {PostalCode}, {Country}";
    }
}
