using MediatR;
using FamilySafety.Application.Common;
using FamilySafety.Application.Common.Interfaces;
using FamilySafety.Application.Queries;
using FamilySafety.Domain.Entities;
using FamilySafety.Shared.DTOs;

namespace FamilySafety.Application.QueryHandlers;

/// <summary>
/// Handler for getting latest locations of family members
/// </summary>
public class GetFamilyLocationsQueryHandler : IRequestHandler<GetFamilyLocationsQuery, Result<IEnumerable<LocationDto>>>
{
    private readonly IRepository<FamilyMember> _familyMemberRepository;
    private readonly IRepository<LocationHistory> _locationRepository;

    public GetFamilyLocationsQueryHandler(
        IRepository<FamilyMember> familyMemberRepository,
        IRepository<LocationHistory> locationRepository)
    {
        _familyMemberRepository = familyMemberRepository;
        _locationRepository = locationRepository;
    }

    public async Task<Result<IEnumerable<LocationDto>>> Handle(GetFamilyLocationsQuery request, CancellationToken cancellationToken)
    {
        // Get all family members with location sharing enabled
        var familyMembers = _familyMemberRepository
            .Query()
            .Where(fm => fm.FamilyGroupId == request.FamilyGroupId && fm.IsLocationSharingEnabled)
            .Select(fm => fm.UserId)
            .ToList();

        if (!familyMembers.Any())
        {
            return Result<IEnumerable<LocationDto>>.Success(Enumerable.Empty<LocationDto>());
        }

        // Get the latest location for each family member
        var latestLocations = _locationRepository
            .Query()
            .Where(lh => familyMembers.Contains(lh.UserId))
            .AsEnumerable()
            .GroupBy(lh => lh.UserId)
            .Select(g => g.OrderByDescending(lh => lh.RecordedAt).First())
            .Select(lh => new LocationDto
            {
                UserId = lh.UserId,
                Latitude = lh.Latitude,
                Longitude = lh.Longitude,
                Accuracy = lh.Accuracy,
                BatteryLevel = lh.BatteryLevel,
                Address = lh.Address,
                RecordedAt = lh.RecordedAt
            })
            .ToList();

        return Result<IEnumerable<LocationDto>>.Success(latestLocations);
    }
}
