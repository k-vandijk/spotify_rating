using Microsoft.AspNetCore.Http;
using spotify_rating.Data.Entities;

namespace spotify_rating.Data.Repositories;

public interface ITrackRepository : IBaseRepository<Track>
{
}

public class TrackRepository : BaseRepository<Track>, ITrackRepository
{
    public TrackRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
    {
    }
}