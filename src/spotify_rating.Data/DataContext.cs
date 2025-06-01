using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;

namespace spotify_rating.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<UserTrack> UserTracks { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
}