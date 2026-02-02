using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Queries.GetUserProfile;

/// <summary>
/// Handler for GetUserProfileQuery
/// </summary>
public sealed class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;
    private readonly ILogger<GetUserProfileQueryHandler> _logger;

    public GetUserProfileQueryHandler(
        IUserRepository userRepository,
        IRoleService roleService,
        ILogger<GetUserProfileQueryHandler> logger
    )
    {
        _userRepository = userRepository;
        _roleService = roleService;
        _logger = logger;
    }

    public async Task<UserDto?> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByCognitoIdAsync(request.CognitoId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User not found for CognitoId: {CognitoId}", request.CognitoId);
            return null;
        }

        _logger.LogInformation(
            "User found: Id={Id}, CognitoId={CognitoId}, RoleId={RoleId}",
            user.Id,
            user.CognitoId.Value,
            user.RoleId
        );

        // Try to get role, but don't fail if role service has issues
        try
        {
            var roleDto = await _roleService.GetRoleByIdAsync(user.RoleId, cancellationToken);
            if (roleDto is null)
            {
                _logger.LogWarning("Role not found for RoleId: {RoleId}", user.RoleId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching role for RoleId: {RoleId}", user.RoleId);
        }

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
