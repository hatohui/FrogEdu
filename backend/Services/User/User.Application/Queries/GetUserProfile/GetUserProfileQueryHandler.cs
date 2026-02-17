using FrogEdu.Shared.Kernel.Authorization;
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
    private readonly ICognitoAttributeService _cognitoAttributeService;
    private readonly ILogger<GetUserProfileQueryHandler> _logger;

    public GetUserProfileQueryHandler(
        IUserRepository userRepository,
        IRoleService roleService,
        ICognitoAttributeService cognitoAttributeService,
        ILogger<GetUserProfileQueryHandler> logger
    )
    {
        _userRepository = userRepository;
        _roleService = roleService;
        _cognitoAttributeService = cognitoAttributeService;
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

        // Map RoleId to role name
        var roleName = RoleConstants.MapRoleIdToName(user.RoleId);

        // Self-healing: Sync role to Cognito's custom:role attribute.
        // This ensures the JWT will carry the correct role on subsequent requests,
        // eliminating the need for other microservices to call the User service for role info.
        // Awaited but non-blocking to the user — errors are logged, not thrown.
        try
        {
            var syncResult = await _cognitoAttributeService.SyncRoleAttributeAsync(
                request.CognitoId,
                roleName,
                cancellationToken
            );

            if (syncResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to sync role to Cognito for user {CognitoId}: {Error}",
                    request.CognitoId,
                    syncResult.Error
                );
            }
            else
            {
                _logger.LogInformation(
                    "Successfully synced role '{Role}' to Cognito for user {CognitoId}",
                    roleName,
                    request.CognitoId
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception syncing role to Cognito for user {CognitoId}",
                request.CognitoId
            );
        }

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
