using FamilySafety.Application.Common.Interfaces;

namespace FamilySafety.Infrastructure.Services;

/// <summary>
/// Service for providing current date/time (mockable for testing)
/// </summary>
public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
