using spotify_rating.Data.Entities;

namespace spotify_rating.Web.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Track> Tracks { get; set; }
    public int TotalTracks;
    public int RatedTracks;
}