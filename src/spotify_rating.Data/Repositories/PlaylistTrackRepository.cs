using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using spotify_rating.Data.Entities;

namespace spotify_rating.Data.Repositories;

public interface IPlaylistTrackRepository : IBaseRepository<PlaylistTrack>
{
}

public class PlaylistTrackRepository : BaseRepository<PlaylistTrack>, IPlaylistTrackRepository
{
    public PlaylistTrackRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache) : base(context, httpContextAccessor, memoryCache)
    {
    }
}