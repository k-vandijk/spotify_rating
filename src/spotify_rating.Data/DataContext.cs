using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;

namespace spotify_rating.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<TrafficLog> TrafficLogs { get; set; }
    public DbSet<UserPlaylist> UserPlaylists { get; set; }
    public DbSet<UserTrack> UserTracks { get; set; }
}