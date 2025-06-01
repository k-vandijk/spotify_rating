using spotify_rating.Data.Entities;

namespace spotify_rating.Data.Repositories;

public interface ITrackRepository : IRepository<Track>
{
}

public class TrackRepository : Repository<Track>, ITrackRepository
{
    public TrackRepository(DataContext context) : base(context)
    {
    }
}