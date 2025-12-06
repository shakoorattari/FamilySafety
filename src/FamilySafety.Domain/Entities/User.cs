using FamilySafety.Domain.Common;
using FamilySafety.Domain.Enums;

namespace FamilySafety.Domain.Entities;

/// <summary>
/// Represents a user in the Family Safety application
/// </summary>
public class User : BaseEntity
{
    public string ExternalId { get; private set; } = string.Empty; // Azure AD B2C Object ID
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public string? ProfileImageUrl { get; private set; }
    public UserStatus Status { get; private set; } = UserStatus.Active;
    public DateTime? LastLoginAt { get; private set; }
    public string? DeviceToken { get; private set; } // For push notifications
    public string? TimeZone { get; private set; }

    // Navigation properties
    public virtual ICollection<FamilyMember> FamilyMemberships { get; private set; } = [];
    public virtual ICollection<LocationHistory> LocationHistories { get; private set; } = [];
    public virtual ICollection<SafetyAlert> SafetyAlerts { get; private set; } = [];

    private User() { }

    public static User Create(string externalId, string email, string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentNullException(nameof(externalId));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));

        return new User
        {
            ExternalId = externalId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Status = UserStatus.Active
        };
    }

    public void UpdateProfile(string firstName, string lastName, string? phoneNumber, string? timeZone)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        TimeZone = timeZone;
    }

    public void UpdateDeviceToken(string? deviceToken)
    {
        DeviceToken = deviceToken;
    }

    public void UpdateProfileImage(string? imageUrl)
    {
        ProfileImageUrl = imageUrl;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        Status = UserStatus.Suspended;
    }

    public void Activate()
    {
        Status = UserStatus.Active;
    }

    public string FullName => $"{FirstName} {LastName}";
}
