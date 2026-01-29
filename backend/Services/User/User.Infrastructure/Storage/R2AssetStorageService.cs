using Amazon.S3;
using Amazon.S3.Model;
using FrogEdu.User.Domain.Services;
using FrogEdu.User.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Infrastructure.Storage;

public class R2AssetStorageService : IAssetStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _publicEndpoint;
    private readonly ILogger<R2AssetStorageService> _logger;

    public R2AssetStorageService(
        IAmazonS3 s3Client,
        IConfiguration configuration,
        ILogger<R2AssetStorageService> logger
    )
    {
        _s3Client = s3Client;
        _logger = logger;

        _bucketName =
            configuration["R2:BucketName"]
            ?? Environment.GetEnvironmentVariable("R2__BucketName")
            ?? Environment.GetEnvironmentVariable("R2_BUCKET_NAME")
            ?? throw new InvalidOperationException("R2:BucketName configuration is missing");

        _publicEndpoint =
            configuration["R2:PublicEndpoint"]
            ?? Environment.GetEnvironmentVariable("R2__PublicEndpoint")
            ?? Environment.GetEnvironmentVariable("R2_PUBLIC_ENDPOINT")
            ?? throw new InvalidOperationException("R2:PublicEndpoint configuration is missing");

        _logger.LogInformation(
            "R2AssetStorageService initialized with bucket: {BucketName}, endpoint: {PublicEndpoint}",
            _bucketName,
            _publicEndpoint
        );
    }

    public Task<PresignedUploadUrl> GenerateUploadUrlAsync(
        AssetUploadRequest request,
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var objectKey = $"{request.Folder}/{userId}/{request.FileName}";

            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(15), // URL valid for 15 minutes
                ContentType = request.ContentType,
            };

            var uploadUrl = _s3Client.GetPreSignedURL(presignedRequest);

            var publicUrl = $"{_publicEndpoint.TrimEnd('/')}/{objectKey}";

            _logger.LogInformation(
                "Generated presigned upload URL for object key: {ObjectKey}",
                objectKey
            );

            return Task.FromResult(
                PresignedUploadUrl.Create(
                    uploadUrl,
                    publicUrl,
                    presignedRequest.Expires ?? DateTime.UtcNow.AddMinutes(15),
                    objectKey
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to generate presigned upload URL for folder {Folder}",
                request.Folder
            );
            throw;
        }
    }
}
