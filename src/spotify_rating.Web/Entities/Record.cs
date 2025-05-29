using CsvHelper.Configuration;
using spotify_rating.Web.Enums;

namespace spotify_rating.Web.Entities;

public class Record : BaseEntity
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string AlbumCoverUrl { get; set; }
    public string UserId { get; set; }
    public RecordRating? Rating { get; set; }
    public DateTime? RatedAtUtc { get; set; }
}

public class RecordMap : ClassMap<Record>
{
    public RecordMap()
    {
        Map(m => m.Title).Name("Track Name");
        Map(m => m.Artist).Name("Artists");
        Map(m => m.AlbumCoverUrl).Name("Album Cover URL");
    }
}