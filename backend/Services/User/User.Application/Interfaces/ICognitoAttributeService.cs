using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Application.Interfaces;

/// <summary>
/// Service for managing Cognito user attributes
/// </summary>
public interface ICognitoAttributeService
{
    /// <summary>
    /// Updates a user's custom:role attribute in Cognito to sync with database role
    /// </summary>
    /// <param name="cognitoId">The user's Cognito ID (sub)</param>
    /// <param name="roleName">The role name (Admin, Teacher, Student)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result indicating success or failure</returns>
    Task<Result> SyncRoleAttributeAsync(
        string cognitoId,
        string roleName,
        CancellationToken cancellationToken = default
    );
}
