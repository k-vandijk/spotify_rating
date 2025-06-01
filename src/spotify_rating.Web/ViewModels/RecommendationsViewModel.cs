using spotify_rating.Data.Entities;

namespace spotify_rating.Web.ViewModels;

public class RecommendationsViewModel
{
    public List<Playlist> Playlists { get; set; }
    public List<Track> Tracks { get; set; }
}

