namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// Interface for fetching subscription claims from the Subscription microservice.
/// Implement this in Infrastructure layer to call the Subscription API.
/// </summary>
public interface ISubscriptionClaimsClient
{
    /// <summary>
    /// Get subscription claims for a user by their ID.
    /// </summary>
    /// <param name="userId">The user's unique identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Subscription claims DTO, defaults to free plan on failure</returns>
    Task<SubscriptionClaimsDto> GetSubscriptionClaimsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
}
