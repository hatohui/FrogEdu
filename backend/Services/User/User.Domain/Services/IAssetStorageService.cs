using FrogEdu.User.Domain.ValueObjects;

namespace FrogEdu.User.Domain.Services;

public interface IAssetStorageService
{
    Task<PresignedUploadUrl> GenerateUploadUrlAsync(
        AssetUploadRequest request,
        Guid userId,
        CancellationToken cancellationToken = default
    );
}
