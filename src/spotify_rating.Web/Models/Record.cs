using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace spotify_rating.Web.Models;

public class Record
{
    [Name("Track Name")]
    public string TrackName { get; set; }

    [Name("Artist")]
    public string Artist { get; set; }

    [Name("Album Cover URL")]
    public string AlbumCoverUrl { get; set; }
}

public class RecordMap : ClassMap<Record>
{
    public RecordMap()
    {
        Map(m => m.TrackName).Name("Track Name");
        Map(m => m.Artist).Name("Artists");
        Map(m => m.AlbumCoverUrl).Name("Album Cover URL");
    }
}