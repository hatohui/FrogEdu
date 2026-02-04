using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// HTTP client implementation for fetching subscription claims from the Subscription microservice.
/// This is a reusable implementation that can be registered in any microservice.
/// </summary>
public sealed class SubscriptionClaimsHttpClient : ISubscriptionClaimsClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SubscriptionClaimsHttpClient> _logger;
    private readonly string _subscriptionServiceUrl;

    public SubscriptionClaimsHttpClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<SubscriptionClaimsHttpClient> logger
    )
    {
        _httpClient = httpClient;
        _logger = logger;

        // Get subscription service URL from configuration with fallback to environment variable
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
            var requestUrl = $"{_subscriptionServiceUrl}/claims/{userId}";

            _logger.LogDebug(
                "Fetching subscription claims for user {UserId} from {Url}",
                userId,
                requestUrl
            );

            var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Failed to fetch subscription claims for user {UserId}. Status: {StatusCode}",
                    userId,
                    response.StatusCode
                );

                return SubscriptionClaimsDto.FreePlan(userId);
            }

            var claims = await response.Content.ReadFromJsonAsync<SubscriptionClaimsDto>(
                cancellationToken: cancellationToken
            );

            if (claims is null)
            {
                _logger.LogWarning("Received null subscription claims for user {UserId}", userId);

                return SubscriptionClaimsDto.FreePlan(userId);
            }

            _logger.LogDebug(
                "Retrieved subscription claims for user {UserId}: Plan={Plan}, Active={Active}",
                userId,
                claims.Plan,
                claims.HasActiveSubscription
            );

            return claims;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "HTTP error fetching subscription claims for user {UserId}",
                userId
            );

            return SubscriptionClaimsDto.FreePlan(userId);
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken != cancellationToken)
        {
            _logger.LogWarning("Timeout fetching subscription claims for user {UserId}", userId);

            return SubscriptionClaimsDto.FreePlan(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error fetching subscription claims for user {UserId}",
                userId
            );

            return SubscriptionClaimsDto.FreePlan(userId);
        }
    }
}
