namespace FamilySafety.Application.Common.Interfaces;

/// <summary>
/// Interface for sending push notifications
/// </summary>
public interface INotificationService
{
    Task SendPushNotificationAsync(string deviceToken, string title, string message, Dictionary<string, string>? data = null, CancellationToken cancellationToken = default);
    Task SendPushNotificationToUsersAsync(IEnumerable<string> deviceTokens, string title, string message, Dictionary<string, string>? data = null, CancellationToken cancellationToken = default);
    Task SendPushNotificationToFamilyAsync(Guid familyGroupId, string title, string message, Dictionary<string, string>? data = null, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for real-time communication
/// </summary>
public interface IRealtimeService
{
    Task SendLocationUpdateAsync(Guid familyGroupId, Guid userId, double latitude, double longitude, CancellationToken cancellationToken = default);
    Task SendAlertAsync(Guid familyGroupId, Guid alertId, string title, string message, CancellationToken cancellationToken = default);
    Task NotifyMemberOnlineStatusAsync(Guid familyGroupId, Guid userId, bool isOnline, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for blob storage operations
/// </summary>
public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<string> GetBlobUrlAsync(string containerName, string blobName);
}

/// <summary>
/// Interface for geocoding service
/// </summary>
public interface IGeocodingService
{
    Task<string?> GetAddressFromCoordinatesAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
    Task<(double Latitude, double Longitude)?> GetCoordinatesFromAddressAsync(string address, CancellationToken cancellationToken = default);
}
