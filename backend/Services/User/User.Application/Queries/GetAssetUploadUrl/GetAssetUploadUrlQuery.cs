using FrogEdu.Shared.Kernel;
using FrogEdu.User.Domain.Services;
using FrogEdu.User.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Queries.GetAssetUploadUrl;

/// <summary>
/// Query to get a presigned URL for asset upload
/// </summary>
public record GetAssetUploadUrlQuery(string Folder, string? ContentType = null)
    : IRequest<Result<AssetUploadUrlResponse>>;

/// <summary>
/// Response containing presigned upload URL and public URL
/// </summary>
public record AssetUploadUrlResponse(
    string UploadUrl,
    string PublicUrl,
    DateTime ExpiresAt,
    string ObjectKey
);

/// <summary>
/// Handler for GetAssetUploadUrlQuery
/// </summary>
public class GetAssetUploadUrlQueryHandler
    : IRequestHandler<GetAssetUploadUrlQuery, Result<AssetUploadUrlResponse>>
{
    private readonly IAssetStorageService _assetStorageService;
    private readonly ILogger<GetAssetUploadUrlQueryHandler> _logger;

    public GetAssetUploadUrlQueryHandler(
        IAssetStorageService assetStorageService,
        ILogger<GetAssetUploadUrlQueryHandler> logger
    )
    {
        _assetStorageService = assetStorageService;
        _logger = logger;
    }

    public async Task<Result<AssetUploadUrlResponse>> Handle(
        GetAssetUploadUrlQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            // Generate a unique file name
            var fileName = $"{Guid.NewGuid()}{GetFileExtension(request.ContentType)}";

            var uploadRequest = AssetUploadRequest.Create(
                request.Folder,
                fileName,
                request.ContentType ?? "application/octet-stream"
            );

            // For now, use a placeholder user ID - in production, get from claims
            var userId = Guid.NewGuid();

            var presignedUrl = await _assetStorageService.GenerateUploadUrlAsync(
                uploadRequest,
                userId,
                cancellationToken
            );

            _logger.LogInformation(
                "Generated presigned upload URL for folder {Folder}",
                request.Folder
            );

            return Result<AssetUploadUrlResponse>.Success(
                new AssetUploadUrlResponse(
                    presignedUrl.UploadUrl,
                    presignedUrl.PublicUrl,
                    presignedUrl.ExpiresAt,
                    presignedUrl.ObjectKey
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate presigned upload URL");
            return Result<AssetUploadUrlResponse>.Failure("Failed to generate upload URL");
        }
    }

    private static string GetFileExtension(string? contentType)
    {
        return contentType?.ToLower() switch
        {
            "image/jpeg" => ".jpg",
            "image/jpg" => ".jpg",
            "image/png" => ".png",
            "image/gif" => ".gif",
            "image/webp" => ".webp",
            "image/svg+xml" => ".svg",
            "application/pdf" => ".pdf",
            "video/mp4" => ".mp4",
            "video/webm" => ".webm",
            _ => "",
        };
    }
}
