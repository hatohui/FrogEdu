using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetUserProfile;

/// <summary>
/// Handler for GetUserProfileQuery
/// </summary>
public sealed class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByCognitoIdAsync(request.CognitoId, cancellationToken);

        if (user is null)
            return null;

        return new UserDto(
            Id: user.Id,
            CognitoId: user.CognitoId.Value,
            Email: user.Email.Value,
            FirstName: user.FullName.FirstName,
            LastName: user.FullName.LastName,
            Role: user.Role.ToString(),
            AvatarUrl: user.AvatarUrl,
            IsEmailVerified: user.IsEmailVerified,
            LastLoginAt: user.LastLoginAt,
            CreatedAt: user.CreatedAt,
            UpdatedAt: user.UpdatedAt
        );
    }
}
