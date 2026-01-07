using MediatR;
using FamilySafety.Application.Common;
using FamilySafety.Shared.DTOs;

namespace FamilySafety.Application.Queries;

/// <summary>
/// Query to get latest locations of family members
/// </summary>
public record GetFamilyLocationsQuery : IRequest<Result<IEnumerable<LocationDto>>>
{
    public Guid FamilyGroupId { get; init; }
}
