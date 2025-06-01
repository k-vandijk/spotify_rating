using spotify_rating.Data.Enums;

namespace spotify_rating.Data.Entities;

public class UserTrack : BaseEntity
{
    public string SpotifyUserId { get; set; }

    public Guid TrackId { get; set; }
    public virtual Track Track { get; set; }
    
    public TrackRating? Rating { get; set; }
    public DateTime? RatedAtUtc { get; set; }
}
