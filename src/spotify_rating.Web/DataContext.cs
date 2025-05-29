using Microsoft.EntityFrameworkCore;
using spotify_rating.Web.Entities;

namespace spotify_rating.Web;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Record> Records { get; set; }
}