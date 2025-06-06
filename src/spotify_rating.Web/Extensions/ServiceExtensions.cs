using spotify_rating.Services;
using spotify_rating.Web.Handlers;

namespace spotify_rating.Web.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddMemoryCache();

        services.AddScoped<IOpenaiService, OpenaiService>();
        services.AddScoped<ISpotifyService, SpotifyService>();

        services.AddHttpClient<ISpotifyService, SpotifyService>()
            .AddHttpMessageHandler<SpotifyAuthHandler>();

        return services;
    }
}