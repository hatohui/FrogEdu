namespace FrogEdu.User.Application.Interfaces;

/// <summary>
/// Interface for S3 storage operations
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Generate a presigned URL for uploading an avatar
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="contentType">Content type of the file (e.g., image/jpeg)</param>
    /// <param name="expirationMinutes">Expiration time in minutes</param>
    /// <returns>Tuple containing upload URL and the final avatar URL</returns>
    Task<(string UploadUrl, string AvatarUrl)> GenerateAvatarUploadUrlAsync(
        Guid userId,
        string contentType,
        int expirationMinutes = 15
    );
}
