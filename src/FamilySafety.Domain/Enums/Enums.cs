namespace FamilySafety.Domain.Enums;

/// <summary>
/// Family member roles within a family group
/// </summary>
public enum FamilyRole
{
    Admin = 0,
    Parent = 1,
    Guardian = 2,
    Child = 3,
    Member = 4
}

/// <summary>
/// Alert types for safety notifications
/// </summary>
public enum AlertType
{
    SOS = 0,
    PanicButton = 1,
    GeofenceBreach = 2,
    LowBattery = 3,
    DeviceOffline = 4,
    SpeedAlert = 5,
    LocationRequest = 6
}

/// <summary>
/// Alert priority levels
/// </summary>
public enum AlertPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

/// <summary>
/// Alert status tracking
/// </summary>
public enum AlertStatus
{
    Active = 0,
    Acknowledged = 1,
    Resolved = 2,
    Expired = 3
}

/// <summary>
/// Geofence types
/// </summary>
public enum GeofenceType
{
    Home = 0,
    School = 1,
    Work = 2,
    SafeZone = 3,
    DangerZone = 4,
    Custom = 5
}

/// <summary>
/// User status
/// </summary>
public enum UserStatus
{
    Active = 0,
    Inactive = 1,
    Suspended = 2,
    Pending = 3
}

/// <summary>
/// Invitation status
/// </summary>
public enum InvitationStatus
{
    Pending = 0,
    Accepted = 1,
    Declined = 2,
    Expired = 3,
    Cancelled = 4
}
