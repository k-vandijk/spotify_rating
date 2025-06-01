namespace spotify_rating.Data.Entities;

public class Playlist : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
}