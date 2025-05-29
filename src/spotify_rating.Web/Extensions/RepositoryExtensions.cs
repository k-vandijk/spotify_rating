using spotify_rating.Web.Repositories;

namespace spotify_rating.Web.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRecordRepository, RecordRepository>();

        return services;
    }
}