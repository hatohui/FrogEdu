using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Shared.Kernel.Authorization;

/// <summary>
/// HTTP client implementation for fetching user role claims from the User microservice.
/// Follows the same pattern as <see cref="SubscriptionClaimsHttpClient"/>.
/// </summary>
public sealed class RoleClaimsHttpClient : IRoleClaimsClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RoleClaimsHttpClient> _logger;
    private readonly string _userServiceUrl;

    public RoleClaimsHttpClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<RoleClaimsHttpClient> logger
    )
    {
        _httpClient = httpClient;
        _logger = logger;

        _userServiceUrl =
            configuration["Services:UserService:Url"]
            ?? Environment.GetEnvironmentVariable("USER_SERVICE_URL")
            ?? "http://localhost:5001/api/users";
    }

    public async Task<RoleClaimsDto> GetRoleClaimsAsync(
        string cognitoSub,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var requestUrl = $"{_userServiceUrl}/by-cognito/{cognitoSub}";

            _logger.LogDebug(
                "Fetching role claims for CognitoSub {CognitoSub} from {Url}",
                cognitoSub,
                requestUrl
            );

            var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Failed to fetch user profile for CognitoSub {CognitoSub}. Status: {StatusCode}",
                    cognitoSub,
                    response.StatusCode
                );
                return RoleClaimsDto.Default(cognitoSub);
            }

            var userDto = await response.Content.ReadFromJsonAsync<UserProfileResponse>(
                cancellationToken: cancellationToken
            );

            if (userDto is null)
            {
                _logger.LogWarning(
                    "Received null user profile for CognitoSub {CognitoSub}",
                    cognitoSub
                );
                return RoleClaimsDto.Default(cognitoSub);
            }

            var roleName = RoleConstants.MapRoleIdToName(userDto.RoleId);

            _logger.LogDebug(
                "Retrieved role {Role} for CognitoSub {CognitoSub} (UserId: {UserId})",
                roleName,
                cognitoSub,
                userDto.Id
            );

            return new RoleClaimsDto
            {
                CognitoSub = cognitoSub,
                UserId = userDto.Id,
                RoleId = userDto.RoleId,
                Role = roleName,
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "HTTP error fetching role claims for CognitoSub {CognitoSub}",
                cognitoSub
            );
            return RoleClaimsDto.Default(cognitoSub);
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken != cancellationToken)
        {
            _logger.LogWarning(
                "Timeout fetching role claims for CognitoSub {CognitoSub}",
                cognitoSub
            );
            return RoleClaimsDto.Default(cognitoSub);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error fetching role claims for CognitoSub {CognitoSub}",
                cognitoSub
            );
            return RoleClaimsDto.Default(cognitoSub);
        }
    }

    /// <summary>
    /// Internal DTO matching the User service /by-cognito/{id} response shape
    /// </summary>
    private sealed record UserProfileResponse(
        Guid Id,
        string CognitoId,
        string Email,
        string FirstName,
        string LastName,
        Guid RoleId,
        string? AvatarUrl,
        bool IsEmailVerified,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
