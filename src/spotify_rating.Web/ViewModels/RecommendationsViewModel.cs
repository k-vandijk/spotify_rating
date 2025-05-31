namespace spotify_rating.Web.ViewModels;

public class RecommendationsViewModel
{
    public List<AiPlaylistViewModel> Playlists { get; set; } = new();
}

public class AiPlaylistViewModel
{
    public string PlaylistName { get; set; }
    public string PlaylistDescription { get; set; }
    public List<AiTrackViewModel> Tracks { get; set; } = new();
}

public class AiTrackViewModel
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string RecommendationReason { get; set; }
}