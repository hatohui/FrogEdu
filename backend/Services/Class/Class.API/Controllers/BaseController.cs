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

        // Debug logging
        var logger = HttpContext.RequestServices.GetService<ILogger<BaseController>>();
        if (logger != null)
        {
            var allClaims = string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}"));
            logger.LogInformation("JWT Claims: {Claims}", allClaims);
            logger.LogInformation("Extracted UserId: '{UserId}'", userId);
        }

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        return userId;
    }

    /// <summary>
    /// Get user role from JWT token claims.
    /// The JWT's custom:role claim is mapped to ClaimTypes.Role during authentication.
    /// Falls back to checking cognito:groups if Role claim is not present.
    /// </summary>
    /// <returns>User role (Admin, Teacher, or Student)</returns>
    protected string GetUserRole()
    {
        // Check standard Role claim (JWT custom:role is mapped here during token validation)
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (!string.IsNullOrEmpty(role))
        {
            return role;
        }

        // Fallback: Check Cognito groups (can be multiple)
        var groups = GetUserGroups().ToList();
        if (groups.Contains("Admin"))
        {
            return "Admin";
        }
        if (groups.Contains("Teacher"))
        {
            return "Teacher";
        }
        if (groups.Contains("Student"))
        {
            return "Student";
        }

        // Default to Student if no role found
        return "Student";
    }
}
