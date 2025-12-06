using FamilySafety.Domain.Common;
using FamilySafety.Domain.Enums;

namespace FamilySafety.Domain.Entities;

/// <summary>
/// Represents a user's membership in a family group
/// </summary>
public class FamilyMember : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid FamilyGroupId { get; private set; }
    public FamilyRole Role { get; private set; }
    public bool IsLocationSharingEnabled { get; private set; } = true;
    public bool CanViewOtherMembersLocation { get; private set; } = true;
    public bool ReceiveAlerts { get; private set; } = true;
    public DateTime JoinedAt { get; private set; }

    // Navigation properties
    public virtual User User { get; private set; } = null!;
    public virtual FamilyGroup FamilyGroup { get; private set; } = null!;

    private FamilyMember() { }

    public static FamilyMember Create(Guid userId, Guid familyGroupId, FamilyRole role)
    {
        return new FamilyMember
        {
            UserId = userId,
            FamilyGroupId = familyGroupId,
            Role = role,
            JoinedAt = DateTime.UtcNow
        };
    }

    public void UpdateRole(FamilyRole role)
    {
        Role = role;
    }

    public void ToggleLocationSharing(bool enabled)
    {
        IsLocationSharingEnabled = enabled;
    }

    public void ToggleViewOtherMembersLocation(bool enabled)
    {
        CanViewOtherMembersLocation = enabled;
    }

    public void ToggleReceiveAlerts(bool enabled)
    {
        ReceiveAlerts = enabled;
    }

    public bool IsAdmin => Role == FamilyRole.Admin;
    public bool IsParentOrGuardian => Role is FamilyRole.Admin or FamilyRole.Parent or FamilyRole.Guardian;
}
