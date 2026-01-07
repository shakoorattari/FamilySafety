using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using FamilySafety.Application.Commands;
using FamilySafety.Shared.DTOs;

namespace FamilySafety.API.Controllers;

/// <summary>
/// Controller for location tracking operations
/// </summary>
[ApiVersion("1.0")]
public class LocationsController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly ILogger<LocationsController> _logger;

    public LocationsController(IMediator mediator, ILogger<LocationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Update current user's location
    /// </summary>
    [HttpPost]
    //[Authorize] // Temporarily disabled for testing
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationRequest request)
    {
        // For testing, use a fixed user id
        var userId = Guid.Parse("12345678-1234-1234-1234-123456789abc");

        var command = new UpdateLocationCommand
        {
            UserId = userId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Altitude = request.Altitude,
            Accuracy = request.Accuracy,
            Speed = request.Speed,
            Bearing = request.Bearing,
            BatteryLevel = request.BatteryLevel
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            _logger.LogInformation("Location updated for user {UserId}: Lat={Latitude}, Lon={Longitude}", userId, request.Latitude, request.Longitude);
            return Ok(new { Message = "Location updated successfully" });
        }
        else
        {
            _logger.LogError("Failed to update location for user {UserId}: {Error}", userId, result.Error);
            return BadRequest(new { Error = result.Error });
        }
    }

    /// <summary>
    /// Get latest locations of family members
    /// </summary>
    [HttpGet("family/{familyGroupId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFamilyLocations(Guid familyGroupId)
    {
        _logger.LogInformation("Getting family locations for group {FamilyGroupId}", familyGroupId);
        // TODO: Implement with MediatR query
        return Ok(new { Message = $"Locations for family {familyGroupId}" });
    }

    /// <summary>
    /// Get location history for a user
    /// </summary>
    [HttpGet("history/{userId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetLocationHistory(Guid userId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        _logger.LogInformation("Getting location history for user {UserId}", userId);
        // TODO: Implement with MediatR query
        return Ok(new { Message = $"Location history for user {userId}" });
    }
}
