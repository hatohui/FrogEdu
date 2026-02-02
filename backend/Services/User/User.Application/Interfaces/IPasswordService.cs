using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Application.Interfaces;

/// <summary>
/// Service for managing user passwords
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// Updates a user's password using their Cognito ID
    /// </summary>
    /// <param name="cognitoId">The user's Cognito ID</param>
    /// <param name="newPassword">The new password</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result indicating success or failure</returns>
    Task<Result> UpdatePasswordAsync(
        string cognitoId,
        string newPassword,
        CancellationToken cancellationToken = default
    );
}
