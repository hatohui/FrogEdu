namespace FrogEdu.User.Application.DTOs;

public sealed record AvatarPresignedUrlDto(string UploadUrl, string AvatarUrl, DateTime ExpiresAt);
