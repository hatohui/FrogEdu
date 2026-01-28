namespace FrogEdu.Content.API.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UsePathPrefixRewrite(
        this IApplicationBuilder builder,
        string prefix
    )
    {
        return builder.UseMiddleware<PathPrefixMiddleware>(prefix);
    }
}
