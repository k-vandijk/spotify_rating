using System.Net.Http.Headers;
using System.Text.Json;
using spotify_rating.Web.Entities;

namespace spotify_rating.Web.Services;

public interface ISpotifyService
{
    Task<List<Record>> GetLikedTracksAsync(string accessToken, string spotifyUserId);
    IEnumerable<Record> GetNewTracksAsync(List<Record> newList, List<Record> oldList);
    IEnumerable<Record> GetRemovedTracksAsync(List<Record> newList, List<Record> oldList);
}

public class SpotifyService : ISpotifyService
{
    private readonly ILogger<SpotifyService> _logger;
    private readonly HttpClient _httpClient;

    public SpotifyService(HttpClient httpClient, ILogger<SpotifyService> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<List<Record>> GetLikedTracksAsync(string accessToken, string spotifyUserId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var likedTracks = new List<Record>();
        var limit = 50;
        var offset = 0;

        while (true)
        {
            var newTracks = await GetBatchAsync(limit, offset, spotifyUserId);

            likedTracks.AddRange(newTracks);

            if (newTracks.Count < limit)
            {
                break;
            }

            offset += limit;

            await Task.Delay(200); // To avoid hitting rate limits
        }

        _logger.LogInformation($"Fetched {likedTracks.Count} liked tracks from Spotify.");
        return likedTracks;
    }

    public IEnumerable<Record> GetNewTracksAsync(List<Record> newList, List<Record> oldList)
    {
        return newList.Where(nlr => !oldList.Any(olr =>
            string.Equals(nlr.Title, olr.Title, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(nlr.Artist, olr.Artist, StringComparison.OrdinalIgnoreCase)));
    }

    public IEnumerable<Record> GetRemovedTracksAsync(List<Record> newList, List<Record> oldList)
    {
        return oldList.Where(olr => !newList.Any(nlr =>
            string.Equals(nlr.Title, olr.Title, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(nlr.Artist, olr.Artist, StringComparison.OrdinalIgnoreCase)));
    }

    private async Task<List<Record>> GetBatchAsync(int limit, int offset, string spotifyUserId)
    {
        var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/me/tracks?limit={limit}&offset={offset}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching liked tracks: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync();

        var doc = JsonDocument.Parse(json);

        var likedTracks = new List<Record>();

        foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
        {
            var track = item.GetProperty("track");
            likedTracks.Add(new Record
            {
                Title = track.GetProperty("name").GetString(),
                Artist = track.GetProperty("artists")[0].GetProperty("name").GetString(),
                SpotifyAlbumCoverUrl = track.GetProperty("album").GetProperty("images")[0].GetProperty("url").GetString(),
                SpotifyUserId = spotifyUserId,
                SpotifyTrackId = track.GetProperty("id").GetString(),
                SpotifyUri = track.GetProperty("uri").GetString(),
            });
        }

        return likedTracks;
    }
}
