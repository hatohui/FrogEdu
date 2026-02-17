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
        var logger = HttpContext.RequestServices.GetService<ILogger<BaseController>>();

        // Check standard Role claim (JWT custom:role is mapped here during token validation)
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (!string.IsNullOrEmpty(role))
        {
            logger?.LogDebug("Role found in ClaimTypes.Role: {Role}", role);
            return role;
        }

        // Fallback: Check Cognito groups (can be multiple)
        var groups = GetUserGroups().ToList();
        logger?.LogDebug("Cognito groups found: {Groups}", string.Join(", ", groups));

        // Check for Admin first (case-insensitive to handle variations)
        if (groups.Any(g => g.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
        {
            logger?.LogInformation("User identified as Admin via cognito:groups");
            return "Admin";
        }
        if (groups.Any(g => g.Equals("Teacher", StringComparison.OrdinalIgnoreCase)))
        {
            logger?.LogInformation("User identified as Teacher via cognito:groups");
            return "Teacher";
        }
        if (groups.Any(g => g.Equals("Student", StringComparison.OrdinalIgnoreCase)))
        {
            logger?.LogInformation("User identified as Student via cognito:groups");
            return "Student";
        }

        // Check for custom:role claim as additional fallback
        var customRole = User.FindFirst("custom:role")?.Value;
        if (!string.IsNullOrEmpty(customRole))
        {
            logger?.LogInformation("Role found in custom:role claim: {Role}", customRole);
            // Capitalize first letter to match expected format
            return char.ToUpper(customRole[0]) + customRole.Substring(1).ToLower();
        }

        // Log warning when defaulting to Student
        logger?.LogWarning(
            "No role found in claims. ClaimTypes.Role: {RoleClaim}, cognito:groups: {Groups}, custom:role: {CustomRole}. Defaulting to Student. This may indicate role enrichment failed.",
            role ?? "null",
            groups.Any() ? string.Join(", ", groups) : "none",
            customRole ?? "null"
        );

        // Default to Student if no role found
        return "Student";
    }
}
