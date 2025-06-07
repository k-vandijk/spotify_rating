using Microsoft.Extensions.DependencyInjection;
using spotify_rating.Data;
using spotify_rating.Data.Entities;

namespace spotify_rating.Services.Services;

public interface ITrafficLogService
{
    Task LogAsync(TrafficLog log, CancellationToken cancellationToken = default);
}

public class TrafficLogService : ITrafficLogService
{
    private readonly IServiceProvider _serviceProvider;

    public TrafficLogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task LogAsync(TrafficLog log, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        context.TrafficLogs.Add(log);
        await context.SaveChangesAsync(cancellationToken);
    }
}