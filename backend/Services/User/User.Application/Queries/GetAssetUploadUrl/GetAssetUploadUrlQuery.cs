using FrogEdu.Shared.Kernel;
using FrogEdu.User.Domain.Repositories;
using FrogEdu.User.Domain.Services;
using FrogEdu.User.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Queries.GetAssetUploadUrl;

public record GetAssetUploadUrlQuery(string CognitoId, string Folder, string? ContentType = null)
    : IRequest<Result<AssetUploadUrlResponse>>;

public record AssetUploadUrlResponse(
    string UploadUrl,
    string PublicUrl,
    DateTime ExpiresAt,
    string ObjectKey
);

public class GetAssetUploadUrlQueryHandler
    : IRequestHandler<GetAssetUploadUrlQuery, Result<AssetUploadUrlResponse>>
{
    private readonly IAssetStorageService _assetStorageService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetAssetUploadUrlQueryHandler> _logger;

    public GetAssetUploadUrlQueryHandler(
        IAssetStorageService assetStorageService,
        IUserRepository userRepository,
        ILogger<GetAssetUploadUrlQueryHandler> logger
    )
    {
        _assetStorageService = assetStorageService;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<AssetUploadUrlResponse>> Handle(
        GetAssetUploadUrlQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var fileName = $"{Guid.NewGuid()}{GetFileExtension(request.ContentType)}";

            var uploadRequest = AssetUploadRequest.Create(
                request.Folder,
                fileName,
                request.ContentType ?? "application/octet-stream"
            );

            var user = await _userRepository.GetByCognitoIdAsync(
                request.CognitoId,
                cancellationToken
            );
            if (user is null)
            {
                _logger.LogWarning("User not found for CognitoId: {CognitoId}", request.CognitoId);
                return Result<AssetUploadUrlResponse>.Failure("User not found");
            }

            var presignedUrl = await _assetStorageService.GenerateUploadUrlAsync(
                uploadRequest,
                user.Id,
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
