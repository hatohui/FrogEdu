namespace FrogEdu.Subscription.API.Middleware;

public class PathPrefixMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _prefix;

    public PathPrefixMiddleware(RequestDelegate next, string prefix)
    {
        _next = next;
        _prefix = prefix;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? "";

        if (path.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
        {
            context.Request.Path = path.Substring(_prefix.Length);
            if (string.IsNullOrEmpty(context.Request.Path.Value))
            {
                context.Request.Path = "/";
            }
        }
        await _next(context);
    }
}
