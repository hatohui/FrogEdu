namespace FrogEdu.User.Application.Interfaces;

/// <summary>
/// Service for retrieving application configuration values
/// </summary>
public interface IApplicationConfiguration
{
    /// <summary>
    /// Gets the frontend base URL for generating links
    /// </summary>
    string GetFrontendBaseUrl();
}
