using spotify_rating.Data.Dtos;

namespace spotify_rating.Web.ViewModels;

public class RecommendationsViewModel
{
    public List<AiPlaylistDto> Playlists { get; set; } = new();
}

