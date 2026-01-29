namespace FrogEdu.User.Domain.ValueObjects;

/// <summary>
/// Value object representing an asset upload request
/// </summary>
public sealed class AssetUploadRequest
{
    public string Folder { get; private set; }
    public string FileName { get; private set; }
    public string ContentType { get; private set; }

    private AssetUploadRequest(string folder, string fileName, string contentType)
    {
        Folder = folder;
        FileName = fileName;
        ContentType = contentType;
    }

    /// <summary>
    /// Create an asset upload request
    /// </summary>
    public static AssetUploadRequest Create(string folder, string fileName, string contentType)
    {
        if (string.IsNullOrWhiteSpace(folder))
            throw new ArgumentException("Folder cannot be empty", nameof(folder));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty", nameof(fileName));

        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Content type cannot be empty", nameof(contentType));

        // Validate folder name (no leading/trailing slashes, no spaces)
        var cleanFolder = folder.Trim('/').Replace(" ", "-");

        // Validate file name (basic sanitization)
        var cleanFileName = fileName.Trim();

        return new AssetUploadRequest(cleanFolder, cleanFileName, contentType);
    }
}

/// <summary>
/// Value object representing a presigned upload URL response
/// </summary>
public sealed class PresignedUploadUrl
{
    public string UploadUrl { get; private set; }
    public string PublicUrl { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public string ObjectKey { get; private set; }

    private PresignedUploadUrl(
        string uploadUrl,
        string publicUrl,
        DateTime expiresAt,
        string objectKey
    )
    {
        UploadUrl = uploadUrl;
        PublicUrl = publicUrl;
        ExpiresAt = expiresAt;
        ObjectKey = objectKey;
    }

    /// <summary>
    /// Create a presigned upload URL
    /// </summary>
    public static PresignedUploadUrl Create(
        string uploadUrl,
        string publicUrl,
        DateTime expiresAt,
        string objectKey
    )
    {
        if (string.IsNullOrWhiteSpace(uploadUrl))
            throw new ArgumentException("Upload URL cannot be empty", nameof(uploadUrl));

        if (string.IsNullOrWhiteSpace(publicUrl))
            throw new ArgumentException("Public URL cannot be empty", nameof(publicUrl));

        if (string.IsNullOrWhiteSpace(objectKey))
            throw new ArgumentException("Object key cannot be empty", nameof(objectKey));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("Expiration time must be in the future", nameof(expiresAt));

        return new PresignedUploadUrl(uploadUrl, publicUrl, expiresAt, objectKey);
    }
}
