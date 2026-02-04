using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.Shared.Kernel.Authorization;

public static class SubscriptionClaimsClientExtensions
{
    public static IServiceCollection AddSubscriptionClaimsClient(
        this IServiceCollection services,
        Action<HttpClient>? configureClient = null
    )
    {
        var clientBuilder = services.AddHttpClient<
            ISubscriptionClaimsClient,
            SubscriptionClaimsHttpClient
        >(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            configureClient?.Invoke(client);
        });

        return services;
    }

    /// <summary>
    /// Add the subscription claims HTTP client with a custom base URL.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="baseUrl">Base URL of the Subscription service</param>
    /// <example>
    /// services.AddSubscriptionClaimsClient("http://subscription-service:5003/api/subscriptions");
    /// </example>
    public static IServiceCollection AddSubscriptionClaimsClient(
        this IServiceCollection services,
        string baseUrl
    )
    {
        return services.AddSubscriptionClaimsClient(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });
    }
}
