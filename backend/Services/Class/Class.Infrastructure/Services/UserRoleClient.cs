using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Infrastructure.Services;

/// <summary>
/// HTTP client for fetching user role from the User microservice
/// </summary>
public interface IUserRoleClient
{
    Task<string> GetUserRoleAsync(string cognitoId, CancellationToken cancellationToken = default);
}

public sealed class UserRoleClient : IUserRoleClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserRoleClient> _logger;
    private readonly string _userServiceUrl;

    // Known Role IDs from User service
    private static readonly Guid TeacherRoleId = new("a1111111-1111-1111-1111-111111111111");
    private static readonly Guid StudentRoleId = new("b2222222-2222-2222-2222-222222222222");
    private static readonly Guid AdminRoleId = new("c3333333-3333-3333-3333-333333333333");

    public UserRoleClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<UserRoleClient> logger
    )
    {
        _httpClient = httpClient;
        _logger = logger;

        _userServiceUrl =
            configuration["Services:UserService:Url"]
            ?? Environment.GetEnvironmentVariable("USER_SERVICE_URL")
            ?? "http://localhost:5001/api/users";
    }

    public async Task<string> GetUserRoleAsync(
        string cognitoId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            // User service expects path: /users/by-cognito/{cognitoId}
            var requestUrl = $"{_userServiceUrl}/by-cognito/{cognitoId}";

            _logger.LogDebug(
                "Fetching user role for CognitoId {CognitoId} from {Url}",
                cognitoId,
                requestUrl
            );

            var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Failed to fetch user profile for CognitoId {CognitoId}. Status: {StatusCode}",
                    cognitoId,
                    response.StatusCode
                );
                return "Student"; // Default to Student
            }

            var userDto = await response.Content.ReadFromJsonAsync<UserProfileDto>(
                cancellationToken: cancellationToken
            );

            if (userDto is null)
            {
                _logger.LogWarning(
                    "Received null user profile for CognitoId {CognitoId}",
                    cognitoId
                );
                return "Student";
            }

            var roleName = MapRoleIdToName(userDto.RoleId);

            _logger.LogDebug(
                "Retrieved role {Role} for CognitoId {CognitoId}",
                roleName,
                cognitoId
            );

            return roleName;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "HTTP error fetching user role for CognitoId {CognitoId}",
                cognitoId
            );
            return "Student";
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken != cancellationToken)
        {
            _logger.LogWarning("Timeout fetching user role for CognitoId {CognitoId}", cognitoId);
            return "Student";
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error fetching user role for CognitoId {CognitoId}",
                cognitoId
            );
            return "Student";
        }
    }

    private static string MapRoleIdToName(Guid roleId)
    {
        if (roleId == TeacherRoleId)
            return "Teacher";
        if (roleId == StudentRoleId)
            return "Student";
        if (roleId == AdminRoleId)
            return "Admin";

        return "Student"; // Default
    }

    private sealed record UserProfileDto(
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
