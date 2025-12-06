using FamilySafety.Domain.Common;
using FamilySafety.Domain.Enums;
using FamilySafety.Domain.Events;

namespace FamilySafety.Domain.Entities;

/// <summary>
/// Represents a safety alert in the system
/// </summary>
public class SafetyAlert : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid? FamilyGroupId { get; private set; }
    public AlertType Type { get; private set; }
    public AlertPriority Priority { get; private set; }
    public AlertStatus Status { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Message { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }
    public string? Address { get; private set; }
    public DateTime TriggeredAt { get; private set; }
    public DateTime? AcknowledgedAt { get; private set; }
    public Guid? AcknowledgedByUserId { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public Guid? ResolvedByUserId { get; private set; }
    public string? ResolutionNotes { get; private set; }

    // Navigation properties
    public virtual User User { get; private set; } = null!;
    public virtual FamilyGroup? FamilyGroup { get; private set; }

    private SafetyAlert() { }

    public static SafetyAlert Create(
        Guid userId,
        AlertType type,
        string title,
        Guid? familyGroupId = null,
        string? message = null,
        double? latitude = null,
        double? longitude = null,
        string? address = null)
    {
        var priority = DeterminePriority(type);

        var alert = new SafetyAlert
        {
            UserId = userId,
            FamilyGroupId = familyGroupId,
            Type = type,
            Priority = priority,
            Status = AlertStatus.Active,
            Title = title,
            Message = message,
            Latitude = latitude,
            Longitude = longitude,
            Address = address,
            TriggeredAt = DateTime.UtcNow
        };

        alert.AddDomainEvent(new SafetyAlertCreatedEvent(alert.Id, userId, type, priority));

        return alert;
    }

    public void Acknowledge(Guid acknowledgedByUserId)
    {
        if (Status != AlertStatus.Active)
            throw new InvalidOperationException("Only active alerts can be acknowledged");

        Status = AlertStatus.Acknowledged;
        AcknowledgedAt = DateTime.UtcNow;
        AcknowledgedByUserId = acknowledgedByUserId;
    }

    public void Resolve(Guid resolvedByUserId, string? notes = null)
    {
        if (Status == AlertStatus.Resolved)
            throw new InvalidOperationException("Alert is already resolved");

        Status = AlertStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;
        ResolvedByUserId = resolvedByUserId;
        ResolutionNotes = notes;
    }

    public void Expire()
    {
        if (Status == AlertStatus.Resolved)
            throw new InvalidOperationException("Resolved alerts cannot expire");

        Status = AlertStatus.Expired;
    }

    private static AlertPriority DeterminePriority(AlertType type)
    {
        return type switch
        {
            AlertType.SOS => AlertPriority.Critical,
            AlertType.PanicButton => AlertPriority.Critical,
            AlertType.GeofenceBreach => AlertPriority.High,
            AlertType.SpeedAlert => AlertPriority.High,
            AlertType.DeviceOffline => AlertPriority.Medium,
            AlertType.LowBattery => AlertPriority.Low,
            AlertType.LocationRequest => AlertPriority.Low,
            _ => AlertPriority.Medium
        };
    }
}
