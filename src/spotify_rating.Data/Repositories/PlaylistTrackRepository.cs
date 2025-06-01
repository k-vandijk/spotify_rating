using spotify_rating.Data.Entities;

namespace spotify_rating.Data.Repositories;

public interface IPlaylistTrackRepository : IRepository<PlaylistTrack>
{
}

public class PlaylistTrackRepository : Repository<PlaylistTrack>, IPlaylistTrackRepository
{
    public PlaylistTrackRepository(DataContext context) : base(context)
    {
    }
}