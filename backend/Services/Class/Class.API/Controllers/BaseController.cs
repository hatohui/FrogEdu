using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Class.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected string? GetCognitoUserId()
    {
        // Try NameIdentifier first (standard claim type), then fall back to "sub" claim
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
    }

    protected string? GetCognitoUsername()
    {
        return User.FindFirstValue("cognito:username");
    }

    /// <summary>
    /// Extract user's email from JWT token
    /// </summary>
    protected string? GetUserEmail()
    {
        return User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email");
    }

    /// <summary>
    /// Extract user's groups from JWT token
    /// </summary>
    protected IEnumerable<string> GetUserGroups()
    {
        return User.FindAll("cognito:groups").Select(c => c.Value);
    }

    /// <summary>
    /// Validates that user is authenticated and returns Cognito User ID
    /// </summary>
    /// <returns>Cognito User ID or throws UnauthorizedAccessException</returns>
    protected string GetAuthenticatedUserId()
    {
        var userId = GetCognitoUserId();
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        return userId;
    }
}
