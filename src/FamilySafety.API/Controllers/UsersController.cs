using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace FamilySafety.API.Controllers;

/// <summary>
/// Controller for user management operations
/// </summary>
[ApiVersion("1.0")]
public class UsersController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        _logger.LogInformation("Getting current user profile");
        // TODO: Implement with MediatR query
        return Ok(new { Message = "User profile endpoint" });
    }

    /// <summary>
    /// Update current user profile
    /// </summary>
    [HttpPut("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        _logger.LogInformation("Updating user profile");
        // TODO: Implement with MediatR command
        return Ok(new { Message = "Profile updated" });
    }

    /// <summary>
    /// Update device token for push notifications
    /// </summary>
    [HttpPut("me/device-token")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateDeviceToken([FromBody] UpdateDeviceTokenRequest request)
    {
        _logger.LogInformation("Updating device token");
        // TODO: Implement with MediatR command
        return Ok(new { Message = "Device token updated" });
    }
}

public record UpdateProfileRequest(string FirstName, string LastName, string? PhoneNumber);
public record UpdateDeviceTokenRequest(string DeviceToken);
