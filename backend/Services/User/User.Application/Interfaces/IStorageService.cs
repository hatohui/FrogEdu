namespace FrogEdu.User.Application.Interfaces;

public interface IStorageService
{
    Task<(string UploadUrl, string AvatarUrl)> GenerateAvatarUploadUrlAsync(
        Guid userId,
        string contentType,
        int expirationMinutes = 15
    );
}
