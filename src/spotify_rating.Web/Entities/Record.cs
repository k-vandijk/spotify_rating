using spotify_rating.Web.Enums;

namespace spotify_rating.Web.Entities;

public class Record : BaseEntity
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string SpotifyAlbumCoverUrl { get; set; }
    public string SpotifyUserId { get; set; }
    public string SpotifyTrackId { get; set; }
    public string SpotifyUri { get; set; }

    public RecordRating? Rating { get; set; }
    public DateTime? RatedAtUtc { get; set; }
}
