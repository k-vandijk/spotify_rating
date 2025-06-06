using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using spotify_rating.Data.Entities;

namespace spotify_rating.Data.Repositories;

public interface IPlaylistRepository : IBaseRepository<Playlist>
{
}

public class PlaylistRepository : BaseRepository<Playlist>, IPlaylistRepository
{
    public PlaylistRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache) : base(context, httpContextAccessor, memoryCache)
    {
    }
}