namespace spotify_rating.Web.ViewModels;

public class RecommendationsViewModel
{
    public List<MusicRecommendation> Recommendations { get; set; } = new List<MusicRecommendation>();
}

public class MusicRecommendation
{
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}

