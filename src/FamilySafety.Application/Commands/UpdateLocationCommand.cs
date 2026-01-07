using MediatR;
using FamilySafety.Application.Common;

namespace FamilySafety.Application.Commands;

/// <summary>
/// Command to update user location
/// </summary>
public record UpdateLocationCommand : IRequest<Result>
{
    public Guid UserId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? Altitude { get; init; }
    public double? Accuracy { get; init; }
    public double? Speed { get; init; }
    public double? Bearing { get; init; }
    public int? BatteryLevel { get; init; }
}