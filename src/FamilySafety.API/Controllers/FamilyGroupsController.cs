using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace FamilySafety.API.Controllers;

/// <summary>
/// Controller for family group management
/// </summary>
[ApiVersion("1.0")]
public class FamilyGroupsController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly ILogger<FamilyGroupsController> _logger;

    public FamilyGroupsController(IMediator mediator, ILogger<FamilyGroupsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all family groups for current user
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyFamilyGroups()
    {
        _logger.LogInformation("Getting family groups for current user");
        // TODO: Implement with MediatR query
        return Ok(new { Message = "Family groups endpoint" });
    }

    /// <summary>
    /// Get a specific family group by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFamilyGroup(Guid id)
    {
        _logger.LogInformation("Getting family group {FamilyGroupId}", id);
        // TODO: Implement with MediatR query
        return Ok(new { Message = $"Family group {id}" });
    }

    /// <summary>
    /// Create a new family group
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateFamilyGroup([FromBody] CreateFamilyGroupRequest request)
    {
        _logger.LogInformation("Creating new family group: {Name}", request.Name);
        // TODO: Implement with MediatR command
        return CreatedAtAction(nameof(GetFamilyGroup), new { id = Guid.NewGuid() }, new { Message = "Family group created" });
    }

    /// <summary>
    /// Join a family group using invite code
    /// </summary>
    [HttpPost("join")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> JoinFamilyGroup([FromBody] JoinFamilyGroupRequest request)
    {
        _logger.LogInformation("Joining family group with code: {InviteCode}", request.InviteCode);
        // TODO: Implement with MediatR command
        return Ok(new { Message = "Joined family group" });
    }

    /// <summary>
    /// Get members of a family group
    /// </summary>
    [HttpGet("{id:guid}/members")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFamilyMembers(Guid id)
    {
        _logger.LogInformation("Getting members for family group {FamilyGroupId}", id);
        // TODO: Implement with MediatR query
        return Ok(new { Message = $"Members of family group {id}" });
    }

    /// <summary>
    /// Regenerate invite code for a family group
    /// </summary>
    [HttpPost("{id:guid}/regenerate-invite")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegenerateInviteCode(Guid id)
    {
        _logger.LogInformation("Regenerating invite code for family group {FamilyGroupId}", id);
        // TODO: Implement with MediatR command
        return Ok(new { InviteCode = "NEW_CODE" });
    }
}

public record CreateFamilyGroupRequest(string Name, string? Description);
public record JoinFamilyGroupRequest(string InviteCode);
