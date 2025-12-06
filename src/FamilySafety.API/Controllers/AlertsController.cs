using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace FamilySafety.API.Controllers;

/// <summary>
/// Controller for safety alerts
/// </summary>
[ApiVersion("1.0")]
public class AlertsController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly ILogger<AlertsController> _logger;

    public AlertsController(IMediator mediator, ILogger<AlertsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Trigger an SOS alert
    /// </summary>
    [HttpPost("sos")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> TriggerSOS([FromBody] TriggerAlertRequest request)
    {
        _logger.LogWarning("SOS Alert triggered! Lat={Latitude}, Lon={Longitude}", request.Latitude, request.Longitude);
        // TODO: Implement with MediatR command
        return CreatedAtAction(nameof(GetAlert), new { id = Guid.NewGuid() }, new { Message = "SOS alert triggered" });
    }

    /// <summary>
    /// Trigger a panic button alert
    /// </summary>
    [HttpPost("panic")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> TriggerPanic([FromBody] TriggerAlertRequest request)
    {
        _logger.LogWarning("Panic Alert triggered! Lat={Latitude}, Lon={Longitude}", request.Latitude, request.Longitude);
        // TODO: Implement with MediatR command
        return CreatedAtAction(nameof(GetAlert), new { id = Guid.NewGuid() }, new { Message = "Panic alert triggered" });
    }

    /// <summary>
    /// Get an alert by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAlert(Guid id)
    {
        _logger.LogInformation("Getting alert {AlertId}", id);
        // TODO: Implement with MediatR query
        return Ok(new { Message = $"Alert {id}" });
    }

    /// <summary>
    /// Get alerts for a family group
    /// </summary>
    [HttpGet("family/{familyGroupId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFamilyAlerts(Guid familyGroupId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        _logger.LogInformation("Getting alerts for family group {FamilyGroupId}", familyGroupId);
        // TODO: Implement with MediatR query
        return Ok(new { Message = $"Alerts for family {familyGroupId}" });
    }

    /// <summary>
    /// Acknowledge an alert
    /// </summary>
    [HttpPost("{id:guid}/acknowledge")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AcknowledgeAlert(Guid id)
    {
        _logger.LogInformation("Acknowledging alert {AlertId}", id);
        // TODO: Implement with MediatR command
        return Ok(new { Message = "Alert acknowledged" });
    }

    /// <summary>
    /// Resolve an alert
    /// </summary>
    [HttpPost("{id:guid}/resolve")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ResolveAlert(Guid id, [FromBody] ResolveAlertRequest? request)
    {
        _logger.LogInformation("Resolving alert {AlertId}", id);
        // TODO: Implement with MediatR command
        return Ok(new { Message = "Alert resolved" });
    }
}

public record TriggerAlertRequest(double? Latitude, double? Longitude, string? Message);
public record ResolveAlertRequest(string? Notes);
