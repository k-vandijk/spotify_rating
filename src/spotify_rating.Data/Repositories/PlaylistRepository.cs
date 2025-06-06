using Microsoft.AspNetCore.Http;
using spotify_rating.Data.Entities;

namespace spotify_rating.Data.Repositories;

public interface IPlaylistRepository : IBaseRepository<Playlist>
{
}

public class PlaylistRepository : BaseRepository<Playlist>, IPlaylistRepository
{
    public PlaylistRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
    {
    }
}