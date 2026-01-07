using MediatR;
using FamilySafety.Application.Common;
using FamilySafety.Application.Common.Interfaces;
using FamilySafety.Application.Queries;
using FamilySafety.Domain.Entities;
using FamilySafety.Shared.DTOs;

namespace FamilySafety.Application.QueryHandlers;

/// <summary>
/// Handler for getting location history for a user
/// </summary>
public class GetLocationHistoryQueryHandler : IRequestHandler<GetLocationHistoryQuery, Result<IEnumerable<LocationDto>>>
{
    private readonly IRepository<LocationHistory> _locationRepository;

    public GetLocationHistoryQueryHandler(IRepository<LocationHistory> locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<Result<IEnumerable<LocationDto>>> Handle(GetLocationHistoryQuery request, CancellationToken cancellationToken)
    {
        var query = _locationRepository
            .Query()
            .Where(lh => lh.UserId == request.UserId);

        // Apply date filters if provided
        if (request.From.HasValue)
        {
            query = query.Where(lh => lh.RecordedAt >= request.From.Value);
        }

        if (request.To.HasValue)
        {
            query = query.Where(lh => lh.RecordedAt <= request.To.Value);
        }

        var locationHistory = query
            .OrderByDescending(lh => lh.RecordedAt)
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

        return Result<IEnumerable<LocationDto>>.Success(locationHistory);
    }
}
