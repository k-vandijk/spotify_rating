using spotify_rating.Data.Repositories;

namespace spotify_rating.Web.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITrackRepository, TrackRepository>();

        return services;
    }
}