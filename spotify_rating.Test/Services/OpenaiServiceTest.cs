using Microsoft.Extensions.Logging.Abstractions;
using spotify_rating.Web.Services;
using Xunit;

namespace spotify_rating.Test.Services;

public class OpenaiServiceTest
{
    private readonly ISpotifyService _service;

    public OpenaiServiceTest()
    {
        var httpClient = new HttpClient();
        var logger = NullLogger<SpotifyService>.Instance;
        _service = new SpotifyService(httpClient, logger);
    }

    [Fact]
    public void GivenTrackIsUnliked_WhenGetRemovedTracksIsCalled_ThenRemovedTrackIsReturned()
    {
        // Arrange
        var oldList = new List<Web.Entities.Record>
        {
            new Web.Entities.Record { Title = "Song A", Artist = "Artist 1" },
            new Web.Entities.Record { Title = "Song B", Artist = "Artist 2" }
        };

        var newList = new List<Web.Entities.Record>
        {
            new Web.Entities.Record { Title = "Song A", Artist = "Artist 1" },
            new Web.Entities.Record { Title = "Song C", Artist = "Artist 3" }
        };

        // Act
        var removedTracks = _service.GetRemovedTracksAsync(newList, oldList).ToList();

        // Assert
        Assert.Single(removedTracks);
        Assert.Equal("Song B", removedTracks[0].Title);
    }

    [Fact]
    public void GivenTrackIsLiked_WhenGetNewTracksIsCalled_ThenNewTrackIsReturned()
    {
        // Arrange
        var oldList = new List<Web.Entities.Record>
        {
            new Web.Entities.Record { Title = "Song A", Artist = "Artist 1" },
            new Web.Entities.Record { Title = "Song B", Artist = "Artist 2" }
        };

        var newList = new List<Web.Entities.Record>
        {
            new Web.Entities.Record { Title = "Song A", Artist = "Artist 1" },
            new Web.Entities.Record { Title = "Song C", Artist = "Artist 3" }
        };

        // Act
        var newTracks = _service.GetNewTracksAsync(newList, oldList).ToList();

        // Assert
        Assert.Single(newTracks);
        Assert.Equal("Song C", newTracks[0].Title);
    }
}