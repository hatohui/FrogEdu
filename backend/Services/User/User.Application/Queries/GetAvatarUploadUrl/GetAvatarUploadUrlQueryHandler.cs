using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetAvatarUploadUrl;

/// <summary>
/// Handler for GetAvatarUploadUrlQuery
/// </summary>
public sealed class GetAvatarUploadUrlQueryHandler
    : IRequestHandler<GetAvatarUploadUrlQuery, AvatarPresignedUrlDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IStorageService _storageService;

    public GetAvatarUploadUrlQueryHandler(
        IUserRepository userRepository,
        IStorageService storageService
    )
    {
        _userRepository = userRepository;
        _storageService = storageService;
    }

    public async Task<AvatarPresignedUrlDto?> Handle(
        GetAvatarUploadUrlQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByCognitoIdAsync(request.CognitoId, cancellationToken);

        if (user is null)
            return null;

        var (uploadUrl, avatarUrl) = await _storageService.GenerateAvatarUploadUrlAsync(
            user.Id,
            request.ContentType,
            15
        );

        return new AvatarPresignedUrlDto(
            UploadUrl: uploadUrl,
            AvatarUrl: avatarUrl,
            ExpiresAt: DateTime.UtcNow.AddMinutes(15)
        );
    }
}
