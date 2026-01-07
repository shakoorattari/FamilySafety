namespace FamilySafety.Shared.DTOs;

/// <summary>
/// User data transfer object
/// </summary>
public record UserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string? ProfileImageUrl { get; init; }
    public string FullName => $"{FirstName} {LastName}";
}

/// <summary>
/// Family group data transfer object
/// </summary>
public record FamilyGroupDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public int MemberCount { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// Family member data transfer object
/// </summary>
public record FamilyMemberDto
{
    public Guid Id { get; init; }
    public UserDto User { get; init; } = null!;
    public string Role { get; init; } = string.Empty;
    public bool IsLocationSharingEnabled { get; init; }
    public DateTime JoinedAt { get; init; }
}

/// <summary>
/// Location data transfer object
/// </summary>
public record LocationDto
{
    public Guid UserId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? Accuracy { get; init; }
    public int? BatteryLevel { get; init; }
    public string? Address { get; init; }
    public DateTime RecordedAt { get; init; }
}

/// <summary>
/// Safety alert data transfer object
/// </summary>
public record SafetyAlertDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? Message { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string? Address { get; init; }
    public DateTime TriggeredAt { get; init; }
}

/// <summary>
/// Geofence data transfer object
/// </summary>
public record GeofenceDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Type { get; init; } = string.Empty;
    public double CenterLatitude { get; init; }
    public double CenterLongitude { get; init; }
    public double RadiusInMeters { get; init; }
    public bool IsActive { get; init; }
    public bool NotifyOnEntry { get; init; }
    public bool NotifyOnExit { get; init; }
}

/// <summary>
/// Update location request
/// </summary>
public record UpdateLocationRequest
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? Altitude { get; init; }
    public double? Accuracy { get; init; }
    public double? Speed { get; init; }
    public double? Bearing { get; init; }
    public int? BatteryLevel { get; init; }
}
