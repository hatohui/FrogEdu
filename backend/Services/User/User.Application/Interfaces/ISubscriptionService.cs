using FrogEdu.Shared.Kernel.Authorization;

namespace FrogEdu.User.Application.Interfaces;

/// <summary>
/// Service for fetching subscription information from the Subscription microservice.
/// This interface extends the shared ISubscriptionClaimsClient for User-specific needs.
/// </summary>
public interface ISubscriptionService : ISubscriptionClaimsClient
{
    // Inherits GetSubscriptionClaimsAsync from ISubscriptionClaimsClient
    // Add any User-specific subscription methods here if needed
}
