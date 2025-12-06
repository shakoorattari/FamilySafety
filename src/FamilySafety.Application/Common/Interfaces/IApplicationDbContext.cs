using FamilySafety.Domain.Entities;

namespace FamilySafety.Application.Common.Interfaces;

/// <summary>
/// Application database context interface
/// </summary>
public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<FamilyGroup> FamilyGroups { get; }
    DbSet<FamilyMember> FamilyMembers { get; }
    DbSet<Geofence> Geofences { get; }
    DbSet<LocationHistory> LocationHistories { get; }
    DbSet<SafetyAlert> SafetyAlerts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Simple DbSet abstraction for the interface
/// </summary>
public interface DbSet<T> : IQueryable<T> where T : class
{
    Task<T?> FindAsync(params object[] keyValues);
    Task<T?> FindAsync(object[] keyValues, CancellationToken cancellationToken);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
