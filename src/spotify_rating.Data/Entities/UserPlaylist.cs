namespace spotify_rating.Data.Entities;

public class UserPlaylist : BaseEntity
{
    public string SpotifyUserId { get; set; } = string.Empty;
    public Guid PlaylistId { get; set; }
    public virtual Playlist Playlist { get; set; }
    public bool IsAiSuggestion { get; set; } = false;
    public bool IsDismissed { get; set; } = false;
    public DateTime? DismissedAtUtc { get; set; } = null;
}