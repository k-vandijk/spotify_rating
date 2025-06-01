using spotify_rating.Data.Repositories;

namespace spotify_rating.Web.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        services.AddScoped<ITrackRepository, TrackRepository>();
        services.AddScoped<IUserPlaylistRepository, UserPlaylistRepository>();
        services.AddScoped<IUserTrackRepository, UserTrackRepository>();

        return services;
    }
}