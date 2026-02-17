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
    protected string GetAuthenticatedUserId()
    {
        var userId = GetCognitoUserId();
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        return userId;
    }

    /// <summary>
    /// Get user role from JWT token claims.
    /// Priority: ClaimTypes.Role (set by OnTokenValidated or RoleEnrichmentMiddleware)
    ///         → cognito:groups → custom:role → default Student.
    /// </summary>
    protected string GetUserRole()
    {
        // Primary: ClaimTypes.Role — set by OnTokenValidated (from custom:role)
        // or by RoleEnrichmentMiddleware (from User service DB lookup)
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (!string.IsNullOrEmpty(role))
        {
            return NormalizeRole(role);
        }

        // Fallback: Cognito groups
        var groups = GetUserGroups().ToList();
        foreach (var expected in new[] { "Admin", "Teacher", "Student" })
        {
            if (groups.Any(g => g.Equals(expected, StringComparison.OrdinalIgnoreCase)))
                return expected;
        }

        // Fallback: Raw custom:role claim (shouldn't reach here if OnTokenValidated ran)
        var customRole = User.FindFirst("custom:role")?.Value;
        if (!string.IsNullOrEmpty(customRole))
        {
            return NormalizeRole(customRole);
        }

        // Default — user should call GET /me to sync their role to Cognito
        return "Student";
    }

    private static string NormalizeRole(string role) => char.ToUpper(role[0]) + role[1..].ToLower();
}
