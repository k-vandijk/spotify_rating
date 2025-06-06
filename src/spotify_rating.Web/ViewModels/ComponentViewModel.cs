namespace spotify_rating.Web.ViewModels;

public class ListItemViewModel
{
    public string PictureUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;

    public string CardUrl { get; set; } = string.Empty;
    public string CalendarText { get; set; } = string.Empty;
    public string SpotifyButtonUrl { get; set; } = string.Empty;
    public string LikeButtonUrl { get; set; } = string.Empty;
    public string DislikeButtonUrl { get; set; } = string.Empty;
    public string Badge { get; set; } = string.Empty;
    public string BadgeClass { get; set; } = "bg-success";
}

public class ListItemMiniViewModel
{
    public string CardUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Badge { get; set; } = string.Empty;
    public string BadgeClass { get; set; } = "bg-success";
}