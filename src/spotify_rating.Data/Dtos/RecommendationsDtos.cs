namespace spotify_rating.Data.Dtos;

public class AiPlaylistDto
{
    public string PlaylistName { get; set; }
    public string PlaylistDescription { get; set; }
    public List<AiTrackDto> Tracks { get; set; } = new();
}

public class AiTrackDto
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string RecommendationReason { get; set; }
}