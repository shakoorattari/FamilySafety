using FamilySafety.Domain.Common;
using FamilySafety.Domain.ValueObjects;

namespace FamilySafety.Domain.Entities;

/// <summary>
/// Represents a user's location at a specific point in time
/// </summary>
public class LocationHistory : BaseEntity
{
    public Guid UserId { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public double? Altitude { get; private set; }
    public double? Accuracy { get; private set; }
    public double? Speed { get; private set; }
    public double? Bearing { get; private set; }
    public int? BatteryLevel { get; private set; }
    public DateTime RecordedAt { get; private set; }
    public string? Address { get; private set; }

    // Navigation properties
    public virtual User User { get; private set; } = null!;

    private LocationHistory() { }

    public static LocationHistory Create(
        Guid userId,
        double latitude,
        double longitude,
        double? altitude = null,
        double? accuracy = null,
        double? speed = null,
        double? bearing = null,
        int? batteryLevel = null,
        string? address = null)
    {
        return new LocationHistory
        {
            UserId = userId,
            Latitude = latitude,
            Longitude = longitude,
            Altitude = altitude,
            Accuracy = accuracy,
            Speed = speed,
            Bearing = bearing,
            BatteryLevel = batteryLevel,
            RecordedAt = DateTime.UtcNow,
            Address = address
        };
    }

    public GeoLocation Location => GeoLocation.Create(Latitude, Longitude, Altitude, Accuracy);
}
