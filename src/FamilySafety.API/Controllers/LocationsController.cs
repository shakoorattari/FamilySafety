using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

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
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationRequest request)
    {
        _logger.LogInformation("Updating location: Lat={Latitude}, Lon={Longitude}", request.Latitude, request.Longitude);
        // TODO: Implement with MediatR command
        return Ok(new { Message = "Location updated" });
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

public record UpdateLocationRequest(
    double Latitude,
    double Longitude,
    double? Altitude,
    double? Accuracy,
    double? Speed,
    int? BatteryLevel);
