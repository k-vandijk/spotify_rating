using spotify_rating.Data.Entities;

namespace spotify_rating.Web.ViewModels;

public class HistoryViewModel
{
    public List<UserTrack> Tracks { get; set; }
    public int Total { get; set; }
    public int Rated { get; set; }
}