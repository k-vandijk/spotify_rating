namespace spotify_rating.Data.Entities;

public class UserPlaylist : BaseEntity
{
    public string SpotifyUserId { get; set; } = string.Empty;
    
    public Guid PlaylistId { get; set; }
    public virtual Playlist Playlist { get; set; }
}