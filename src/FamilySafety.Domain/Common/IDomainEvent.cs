using MediatR;

namespace FamilySafety.Domain.Common;

/// <summary>
/// Marker interface for domain events
/// </summary>
public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}

/// <summary>
/// Base class for domain events
/// </summary>
public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
