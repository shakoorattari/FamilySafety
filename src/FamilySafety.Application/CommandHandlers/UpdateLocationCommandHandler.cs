using MediatR;
using FamilySafety.Application.Common;
using FamilySafety.Application.Common.Interfaces;
using FamilySafety.Domain.Entities;
using FamilySafety.Application.Commands;

namespace FamilySafety.Application.CommandHandlers;

/// <summary>
/// Handler for updating user location
/// </summary>
public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, Result>
{
    private readonly IRepository<LocationHistory> _locationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLocationCommandHandler(
        IRepository<LocationHistory> locationRepository,
        IUnitOfWork unitOfWork)
    {
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var locationHistory = LocationHistory.Create(
            request.UserId,
            request.Latitude,
            request.Longitude,
            request.Altitude,
            request.Accuracy,
            request.Speed,
            request.Bearing,
            request.BatteryLevel,
            null); // address can be null for now

        await _locationRepository.AddAsync(locationHistory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}