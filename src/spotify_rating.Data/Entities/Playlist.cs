namespace spotify_rating.Data.Entities;

public class Playlist : BaseEntity
{
    public string PlaylistName { get; set; }
    public string? PlaylistDescription { get; set; }
    
    public virtual List<Track> Tracks { get; set; } = new();
}