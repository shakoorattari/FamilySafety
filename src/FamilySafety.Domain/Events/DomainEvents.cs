using FamilySafety.Domain.Common;
using FamilySafety.Domain.Enums;

namespace FamilySafety.Domain.Events;

/// <summary>
/// Event raised when a new safety alert is created
/// </summary>
public class SafetyAlertCreatedEvent : DomainEvent
{
    public Guid AlertId { get; }
    public Guid UserId { get; }
    public AlertType AlertType { get; }
    public AlertPriority Priority { get; }

    public SafetyAlertCreatedEvent(Guid alertId, Guid userId, AlertType alertType, AlertPriority priority)
    {
        AlertId = alertId;
        UserId = userId;
        AlertType = alertType;
        Priority = priority;
    }
}

/// <summary>
/// Event raised when a user joins a family group
/// </summary>
public class UserJoinedFamilyEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid FamilyGroupId { get; }
    public FamilyRole Role { get; }

    public UserJoinedFamilyEvent(Guid userId, Guid familyGroupId, FamilyRole role)
    {
        UserId = userId;
        FamilyGroupId = familyGroupId;
        Role = role;
    }
}

/// <summary>
/// Event raised when a geofence breach is detected
/// </summary>
public class GeofenceBreachEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid GeofenceId { get; }
    public bool IsEntry { get; }
    public double Latitude { get; }
    public double Longitude { get; }

    public GeofenceBreachEvent(Guid userId, Guid geofenceId, bool isEntry, double latitude, double longitude)
    {
        UserId = userId;
        GeofenceId = geofenceId;
        IsEntry = isEntry;
        Latitude = latitude;
        Longitude = longitude;
    }
}
