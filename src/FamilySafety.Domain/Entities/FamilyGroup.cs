using FamilySafety.Domain.Common;

namespace FamilySafety.Domain.Entities;

/// <summary>
/// Represents a family group that members can belong to
/// </summary>
public class FamilyGroup : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? ImageUrl { get; private set; }
    public string InviteCode { get; private set; } = string.Empty;
    public DateTime? InviteCodeExpiresAt { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public virtual ICollection<FamilyMember> Members { get; private set; } = [];
    public virtual ICollection<Geofence> Geofences { get; private set; } = [];

    private FamilyGroup() { }

    public static FamilyGroup Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        return new FamilyGroup
        {
            Name = name,
            Description = description,
            InviteCode = GenerateInviteCode(),
            InviteCodeExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
        Description = description;
    }

    public void UpdateImage(string? imageUrl)
    {
        ImageUrl = imageUrl;
    }

    public string RegenerateInviteCode()
    {
        InviteCode = GenerateInviteCode();
        InviteCodeExpiresAt = DateTime.UtcNow.AddDays(7);
        return InviteCode;
    }

    public bool IsInviteCodeValid()
    {
        return !string.IsNullOrEmpty(InviteCode) && 
               InviteCodeExpiresAt.HasValue && 
               InviteCodeExpiresAt.Value > DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    private static string GenerateInviteCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
