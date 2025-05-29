using spotify_rating.Web.Entities;

namespace spotify_rating.Web.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Record> Records { get; set; }
    public int TotalRecords;
    public int RatedRecords;
}