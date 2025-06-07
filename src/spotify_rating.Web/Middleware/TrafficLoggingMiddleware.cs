using Microsoft.AspNetCore.Mvc.Filters;
using spotify_rating.Data.Entities;
using spotify_rating.Services.Services;

namespace spotify_rating.Web.Middleware;

public class TrafficLoggingMiddleware : IActionFilter
{
    private readonly ITrafficLogService _trafficLogService;

    public TrafficLoggingMiddleware(ITrafficLogService trafficLogService)
    {
        _trafficLogService = trafficLogService;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = context.HttpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
        var path = context.HttpContext.Request.Path;
        var method = context.HttpContext.Request.Method;
        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

        var log = new TrafficLog
        {
            UserId = userId,
            Path = path,
            Method = method,
            IPAddress = ip
        };

        _ = Task.Run(() => _trafficLogService.LogAsync(log));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
