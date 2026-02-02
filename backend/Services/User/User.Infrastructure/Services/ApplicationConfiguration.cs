using FrogEdu.User.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FrogEdu.User.Infrastructure.Services;

public sealed class ApplicationConfiguration : IApplicationConfiguration
{
    private readonly IConfiguration _configuration;

    public ApplicationConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetFrontendBaseUrl()
    {
        return _configuration["Frontend:BaseUrl"] ?? "http://localhost:5173";
    }
}
