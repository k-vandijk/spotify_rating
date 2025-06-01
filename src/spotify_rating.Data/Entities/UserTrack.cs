using spotify_rating.Data.Enums;

namespace spotify_rating.Data.Entities;

public class UserTrack : BaseEntity
{
    public string SpotifyUserId { get; set; }
    public Guid TrackId { get; set; }
    public virtual Track Track { get; set; }
    public TrackRating? Rating { get; set; }
    public DateTime? RatedAtUtc { get; set; }
    public bool IsAiSuggestion { get; set; } = false;
    public bool IsDismissed { get; set; } = false;
    public DateTime? DismissedAtUtc { get; set; } = null;
}
