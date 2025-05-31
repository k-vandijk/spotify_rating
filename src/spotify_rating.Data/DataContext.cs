using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;

namespace spotify_rating.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Track> Tracks { get; set; }
}