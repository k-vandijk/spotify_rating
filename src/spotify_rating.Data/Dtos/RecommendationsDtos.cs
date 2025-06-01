namespace spotify_rating.Data.Dtos;

public class AiPlaylistDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public List<AiTrackDto> Tracks { get; set; } = new();
}

public class AiTrackDto
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Genre { get; set; }
}