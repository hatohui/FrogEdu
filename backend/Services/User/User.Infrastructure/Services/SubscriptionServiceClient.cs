using System.Net.Http.Json;
using FrogEdu.Shared.Kernel.Authorization;
using FrogEdu.User.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Infrastructure.Services;

/// <summary>
/// HTTP client service for fetching subscription information from the Subscription microservice.
/// Implements ISubscriptionService which extends ISubscriptionClaimsClient.
/// </summary>
public sealed class SubscriptionServiceClient : ISubscriptionService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SubscriptionServiceClient> _logger;
    private readonly string _subscriptionServiceUrl;

    public SubscriptionServiceClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<SubscriptionServiceClient> logger
    )
    {
        _httpClient = httpClient;
        _logger = logger;

        // Get subscription service URL from configuration
        _subscriptionServiceUrl =
            configuration["Services:SubscriptionService:Url"]
            ?? Environment.GetEnvironmentVariable("SUBSCRIPTION_SERVICE_URL")
            ?? "http://localhost:5003/api/subscriptions";
    }

    public async Task<SubscriptionClaimsDto> GetSubscriptionClaimsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"{_subscriptionServiceUrl}/claims/{userId}",
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Failed to fetch subscription claims for user {UserId}. Status: {StatusCode}",
                    userId,
                    response.StatusCode
                );

                // Return free plan as default
                return SubscriptionClaimsDto.FreePlan(userId);
            }

            var claims = await response.Content.ReadFromJsonAsync<SubscriptionClaimsDto>(
                cancellationToken: cancellationToken
            );

            return claims ?? SubscriptionClaimsDto.FreePlan(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscription claims for user {UserId}", userId);

            // Return free plan as fallback
            return SubscriptionClaimsDto.FreePlan(userId);
        }
    }
}
