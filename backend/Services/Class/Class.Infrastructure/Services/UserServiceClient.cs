using System.Net.Http.Headers;
using System.Net.Http.Json;
using FrogEdu.Class.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Infrastructure.Services;

public sealed class UserServiceClient : IUserServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserServiceClient> _logger;
    private readonly string _userServiceUrl;

    public UserServiceClient(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        ILogger<UserServiceClient> logger
    )
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;

        _userServiceUrl =
            configuration["Services:UserService:Url"]
            ?? Environment.GetEnvironmentVariable("USER_SERVICE_URL")
            ?? "http://localhost:5001/api/users";
    }

    public async Task<UserDto?> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var authHeader =
                _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{_userServiceUrl}/users/{userId}"
            );
            if (!string.IsNullOrEmpty(authHeader))
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(authHeader);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Failed to fetch user {UserId} from User service. Status: {StatusCode}",
                    userId,
                    response.StatusCode
                );
                return null;
            }

            var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>(
                cancellationToken: cancellationToken
            );

            if (userResponse is null)
                return null;

            return new UserDto(
                userResponse.Id,
                userResponse.FirstName,
                userResponse.LastName,
                userResponse.Email,
                userResponse.AvatarUrl
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user {UserId} from User service", userId);
            return null;
        }
    }

    public async Task<IReadOnlyList<UserDto>> GetUsersByIdsAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default
    )
    {
        var users = new List<UserDto>();

        foreach (var userId in userIds)
        {
            var user = await GetUserByIdAsync(userId, cancellationToken);
            if (user is not null)
            {
                users.Add(user);
            }
        }

        return users.AsReadOnly();
    }

    private sealed record UserResponse(
        Guid Id,
        string CognitoId,
        string Email,
        string FirstName,
        string LastName,
        Guid RoleId,
        string? AvatarUrl,
        bool IsEmailVerified,
        bool IsActive
    );
}
