using spotify_rating.Web.Services;

namespace spotify_rating.Web.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddScoped<ISpotifyService, SpotifyService>();

        return services;
    }
}