namespace spotify_rating.Data.Entities;

public class PlaylistTrack : BaseEntity
{
    public Guid PlaylistId { get; set; }
    public Guid TrackId { get; set; }
    public virtual Track Track { get; set; }
}