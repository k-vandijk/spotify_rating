using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using spotify_rating.Data.Entities;

namespace spotify_rating.Services;

public interface ISpotifyService
{
    Task<List<Track>> GetLikedTracksAsync(string accessToken, string spotifyUserId);
    IEnumerable<Track> GetNewTracksAsync(List<Track> newList, List<Track> oldList);
    IEnumerable<Track> GetRemovedTracksAsync(List<Track> newList, List<Track> oldList);
    Task<Track?> GetTrackByTitleAndArtistAsync(string accessToken, string title, string artist, string genre);
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

    public async Task<List<Track>> GetLikedTracksAsync(string accessToken, string spotifyUserId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var likedTracks = new List<Track>();
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

    public IEnumerable<Track> GetNewTracksAsync(List<Track> newList, List<Track> oldList)
    {
        return newList.Where(nlr => !oldList.Any(olr =>
            string.Equals(nlr.Title, olr.Title, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(nlr.Artist, olr.Artist, StringComparison.OrdinalIgnoreCase)));
    }

    public IEnumerable<Track> GetRemovedTracksAsync(List<Track> newList, List<Track> oldList)
    {
        return oldList.Where(olr => !newList.Any(nlr =>
            string.Equals(nlr.Title, olr.Title, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(nlr.Artist, olr.Artist, StringComparison.OrdinalIgnoreCase)));
    }

    public async Task<Track?> GetTrackByTitleAndArtistAsync(string accessToken, string title, string artist, string genre)
    {
        string q = Uri.EscapeDataString(title + " " + artist);

        var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/search?q={q}&type=track&limit=1");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching tracks: {q}");
        }

        var json = await response.Content.ReadAsStringAsync();

        var doc = JsonDocument.Parse(json);

        var items = doc.RootElement.GetProperty("tracks").GetProperty("items");

        if (items.GetArrayLength() == 0)
        {
            return null; // No track found
        }

        var track = items[0];

        return new Track
        {
            Title = track.GetProperty("name").GetString(),
            Artist = track.GetProperty("artists")[0].GetProperty("name").GetString(),
            SpotifyAlbumCoverUrl = track.GetProperty("album").GetProperty("images")[0].GetProperty("url").GetString(),
            SpotifyTrackId = track.GetProperty("id").GetString(),
            SpotifyUri = track.GetProperty("uri").GetString(),
            AiGenre = genre
        };
    }

    private async Task<List<Track>> GetBatchAsync(int limit, int offset, string spotifyUserId)
    {
        var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/me/tracks?limit={limit}&offset={offset}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching liked tracks: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync();

        var doc = JsonDocument.Parse(json);

        var likedTracks = new List<Track>();

        foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
        {
            var track = item.GetProperty("track");
            likedTracks.Add(new Track
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
