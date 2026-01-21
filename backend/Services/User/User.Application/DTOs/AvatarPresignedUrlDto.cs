namespace FrogEdu.User.Application.DTOs;

/// <summary>
/// DTO for avatar upload presigned URL response
/// </summary>
public sealed record AvatarPresignedUrlDto(string UploadUrl, string AvatarUrl, DateTime ExpiresAt);
