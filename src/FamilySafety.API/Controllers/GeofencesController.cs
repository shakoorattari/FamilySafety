using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace FamilySafety.API.Controllers;

/// <summary>
/// Controller for geofence management
/// </summary>
[ApiVersion("1.0")]
public class GeofencesController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly ILogger<GeofencesController> _logger;

    public GeofencesController(IMediator mediator, ILogger<GeofencesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all geofences for a family group
    /// </summary>
    [HttpGet("family/{familyGroupId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGeofences(Guid familyGroupId)
    {
        _logger.LogInformation("Getting geofences for family group {FamilyGroupId}", familyGroupId);
        // TODO: Implement with MediatR query
        return Ok(new { Message = $"Geofences for family {familyGroupId}" });
    }

    /// <summary>
    /// Get a specific geofence by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGeofence(Guid id)
    {
        _logger.LogInformation("Getting geofence {GeofenceId}", id);
        // TODO: Implement with MediatR query
        return Ok(new { Message = $"Geofence {id}" });
    }

    /// <summary>
    /// Create a new geofence
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateGeofence([FromBody] CreateGeofenceRequest request)
    {
        _logger.LogInformation("Creating geofence: {Name}", request.Name);
        // TODO: Implement with MediatR command
        return CreatedAtAction(nameof(GetGeofence), new { id = Guid.NewGuid() }, new { Message = "Geofence created" });
    }

    /// <summary>
    /// Update a geofence
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateGeofence(Guid id, [FromBody] UpdateGeofenceRequest request)
    {
        _logger.LogInformation("Updating geofence {GeofenceId}", id);
        // TODO: Implement with MediatR command
        return Ok(new { Message = "Geofence updated" });
    }

    /// <summary>
    /// Delete a geofence
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteGeofence(Guid id)
    {
        _logger.LogInformation("Deleting geofence {GeofenceId}", id);
        // TODO: Implement with MediatR command
        return NoContent();
    }
}

public record CreateGeofenceRequest(
    Guid FamilyGroupId,
    string Name,
    string? Description,
    string Type,
    double CenterLatitude,
    double CenterLongitude,
    double RadiusInMeters,
    bool NotifyOnEntry,
    bool NotifyOnExit);

public record UpdateGeofenceRequest(
    string Name,
    string? Description,
    double RadiusInMeters,
    bool NotifyOnEntry,
    bool NotifyOnExit,
    bool IsActive);
