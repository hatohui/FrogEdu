using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetUserProfile;

/// <summary>
/// Handler for GetUserProfileQuery
/// </summary>
public sealed class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;

    public GetUserProfileQueryHandler(IUserRepository userRepository, IRoleService roleService)
    {
        _userRepository = userRepository;
        _roleService = roleService;
    }

    public async Task<UserDto?> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByCognitoIdAsync(request.CognitoId, cancellationToken);

        if (user is null)
            return null;

        var roleDto = await _roleService.GetRoleByIdAsync(user.RoleId, cancellationToken);

        return new UserDto(
            Id: user.Id,
            CognitoId: user.CognitoId.Value,
            Email: user.Email.Value,
            FirstName: user.FirstName,
            LastName: user.LastName,
            RoleId: user.RoleId,
            AvatarUrl: user.AvatarUrl,
            IsEmailVerified: user.IsEmailVerified,
            CreatedAt: user.CreatedAt,
            UpdatedAt: user.UpdatedAt
        );
    }
}
