using FrogEdu.User.Domain.ValueObjects;

namespace FrogEdu.User.Domain.Services;

/// <summary>
/// Domain service for asset storage operations
/// </summary>
public interface IAssetStorageService
{
    /// <summary>
    /// Generate a presigned URL for uploading an asset
    /// </summary>
    Task<PresignedUploadUrl> GenerateUploadUrlAsync(
        AssetUploadRequest request,
        Guid userId,
        CancellationToken cancellationToken = default
    );
}
