using spotify_rating.Data.Entities;

namespace spotify_rating.Data.Repositories;

public interface IPlaylistRepository : IRepository<Playlist>
{
}

public class PlaylistRepository : Repository<Playlist>, IPlaylistRepository
{
    public PlaylistRepository(DataContext context) : base(context)
    {
    }
}