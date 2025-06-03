namespace spotify_rating.Web.Utils;

public class ProgressiveFallbackMiddleware
{
    private readonly RequestDelegate _next;

    public ProgressiveFallbackMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalPath = context.Request.Path.ToString().ToLower();

        var routeMatched = await TryMatchRouteAsync(context);

        while (!routeMatched && !string.IsNullOrEmpty(originalPath))
        {
            originalPath = originalPath.Substring(0, originalPath.LastIndexOf('/'));
            context.Request.Path = new PathString(originalPath);

            routeMatched = await TryMatchRouteAsync(context);
        }

        if (!routeMatched)
        {
            context.Request.Path = "/home";
        }

        await _next(context);
    }

    private async Task<bool> TryMatchRouteAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint != null)
        {
            return true;
        }

        return false;
    }
}
