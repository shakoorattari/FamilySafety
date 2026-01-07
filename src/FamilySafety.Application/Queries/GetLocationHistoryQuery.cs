using MediatR;
using FamilySafety.Application.Common;
using FamilySafety.Shared.DTOs;

namespace FamilySafety.Application.Queries;

/// <summary>
/// Query to get location history for a user
/// </summary>
public record GetLocationHistoryQuery : IRequest<Result<IEnumerable<LocationDto>>>
{
    public Guid UserId { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
}
