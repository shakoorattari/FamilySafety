using FamilySafety.Domain.Common;
using FamilySafety.Domain.Enums;
using FamilySafety.Domain.ValueObjects;

namespace FamilySafety.Domain.Entities;

/// <summary>
/// Represents a geofence boundary for location monitoring
/// </summary>
public class Geofence : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public GeofenceType Type { get; private set; }
    public double CenterLatitude { get; private set; }
    public double CenterLongitude { get; private set; }
    public double RadiusInMeters { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool NotifyOnEntry { get; private set; } = true;
    public bool NotifyOnExit { get; private set; } = true;
    public Guid FamilyGroupId { get; private set; }

    // Navigation properties
    public virtual FamilyGroup FamilyGroup { get; private set; } = null!;

    private Geofence() { }

    public static Geofence Create(
        string name,
        GeofenceType type,
        double centerLatitude,
        double centerLongitude,
        double radiusInMeters,
        Guid familyGroupId,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        if (radiusInMeters <= 0)
            throw new ArgumentOutOfRangeException(nameof(radiusInMeters), "Radius must be greater than 0");

        return new Geofence
        {
            Name = name,
            Description = description,
            Type = type,
            CenterLatitude = centerLatitude,
            CenterLongitude = centerLongitude,
            RadiusInMeters = radiusInMeters,
            FamilyGroupId = familyGroupId
        };
    }

    public void Update(string name, string? description, double radiusInMeters)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
        Description = description;
        RadiusInMeters = radiusInMeters;
    }

    public void UpdateCenter(double latitude, double longitude)
    {
        CenterLatitude = latitude;
        CenterLongitude = longitude;
    }

    public void SetNotificationPreferences(bool notifyOnEntry, bool notifyOnExit)
    {
        NotifyOnEntry = notifyOnEntry;
        NotifyOnExit = notifyOnExit;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public GeoLocation Center => GeoLocation.Create(CenterLatitude, CenterLongitude);

    public bool IsLocationInside(GeoLocation location)
    {
        var distance = Center.DistanceTo(location);
        return distance <= RadiusInMeters;
    }
}
