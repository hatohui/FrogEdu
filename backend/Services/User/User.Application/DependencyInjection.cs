using System.Reflection;
using FluentValidation;
using FrogEdu.User.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FrogEdu.User.Application;

/// <summary>
/// Extension methods for configuring Application layer services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add Application layer services to the DI container
    /// </summary>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        string? mediatrLicenseKey = null
    )
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = Environment.GetEnvironmentVariable("MEDIAK_LICENSE_KEY");
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
