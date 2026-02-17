namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Interface for fetching user role claims from the User microservice.
/// Implement this in Infrastructure layer to call the User API.
/// Similar pattern to <see cref="ISubscriptionClaimsClient"/>.
/// </summary>
public interface IRoleClaimsClient
{
    /// <summary>
    /// Get role claims for a user by their Cognito sub (ID).
    /// </summary>
    /// <param name="cognitoSub">The user's Cognito sub claim</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Role claims DTO, defaults to Student on failure</returns>
    Task<RoleClaimsDto> GetRoleClaimsAsync(
        string cognitoSub,
        CancellationToken cancellationToken = default
    );
}
