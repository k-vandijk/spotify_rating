using spotify_rating.Web.Services;
using spotify_rating.Web.Utils;

namespace spotify_rating.Web.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        //services.AddHttpClient();

        services.AddHttpContextAccessor();

        services.AddTransient<SpotifyAuthHandler>();

        services.AddScoped<IOpenaiService, OpenaiService>();
        services.AddScoped<ISpotifyService, SpotifyService>();

        services.AddHttpClient<ISpotifyService, SpotifyService>()
            .AddHttpMessageHandler<SpotifyAuthHandler>();

        return services;
    }
}