using Amazon.S3;
using Amazon.S3.Model;
using FrogEdu.User.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Infrastructure.Services;

/// <summary>
/// S3-based storage service for managing file uploads
/// </summary>
public sealed class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<S3StorageService> _logger;
    private readonly string _bucketName;
    private readonly string _cdnBaseUrl;

    public S3StorageService(
        IAmazonS3 s3Client,
        IConfiguration configuration,
        ILogger<S3StorageService> logger
    )
    {
        _s3Client = s3Client;
        _logger = logger;
        _bucketName = configuration["AWS:S3:BucketName"] ?? "frogedu-assets";
        _cdnBaseUrl =
            configuration["AWS:S3:CdnBaseUrl"] ?? $"https://{_bucketName}.s3.amazonaws.com";
    }

    public async Task<(string UploadUrl, string AvatarUrl)> GenerateAvatarUploadUrlAsync(
        Guid userId,
        string contentType,
        int expirationMinutes = 15
    )
    {
        var extension = GetExtensionFromContentType(contentType);
        var objectKey = $"user-uploads/{userId}/avatar{extension}";

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            ContentType = contentType,
        };

        request.Metadata.Add("x-amz-meta-user-id", userId.ToString());

        var uploadUrl = await _s3Client.GetPreSignedURLAsync(request);
        var avatarUrl = $"{_cdnBaseUrl}/{objectKey}";

        _logger.LogInformation(
            "Generated presigned URL for avatar upload. UserId: {UserId}, ObjectKey: {ObjectKey}",
            userId,
            objectKey
        );

        return (uploadUrl, avatarUrl);
    }

    private static string GetExtensionFromContentType(string contentType)
    {
        return contentType.ToLowerInvariant() switch
        {
            "image/jpeg" or "image/jpg" => ".jpg",
            "image/png" => ".png",
            "image/gif" => ".gif",
            "image/webp" => ".webp",
            _ => ".jpg",
        };
    }
}
