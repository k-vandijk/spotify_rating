using spotify_rating.Data.Enums;

namespace spotify_rating.Data.Entities;

public class Track : BaseEntity
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string SpotifyAlbumCoverUrl { get; set; }
    public string SpotifyUserId { get; set; }
    public string SpotifyTrackId { get; set; }
    public string SpotifyUri { get; set; }
    public string? AiGenre { get; set; } // Optional genre field, can be null if not set

    public TrackRating? Rating { get; set; }
    public DateTime? RatedAtUtc { get; set; }
}
