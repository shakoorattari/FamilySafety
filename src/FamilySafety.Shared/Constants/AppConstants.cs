namespace FamilySafety.Shared.Constants;

/// <summary>
/// Application-wide constants
/// </summary>
public static class AppConstants
{
    public const string ApplicationName = "FamilySafety";
    public const string ApiVersion = "v1";

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Parent = "Parent";
        public const string Guardian = "Guardian";
        public const string Child = "Child";
        public const string Member = "Member";
    }

    public static class Policies
    {
        public const string RequireAdmin = "RequireAdmin";
        public const string RequireParentOrGuardian = "RequireParentOrGuardian";
        public const string RequireFamilyMember = "RequireFamilyMember";
    }

    public static class Claims
    {
        public const string UserId = "user_id";
        public const string FamilyId = "family_id";
        public const string Role = "role";
    }

    public static class Limits
    {
        public const int MaxFamilyMembers = 20;
        public const int MaxGeofencesPerFamily = 50;
        public const int LocationHistoryRetentionDays = 30;
        public const int AlertExpirationHours = 24;
        public const int InviteCodeValidityDays = 7;
    }

    public static class SignalRHubs
    {
        public const string LocationHub = "/hubs/location";
        public const string AlertHub = "/hubs/alert";
    }
}
