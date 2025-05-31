using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Data.Entities;
using spotify_rating.Data.Repositories;
using spotify_rating.Services;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecommendationsController : Controller
{
    private readonly ITrackRepository _trackRepository;
    private readonly IOpenaiService _openaiService;
    private readonly ISpotifyService _spotifyService;

    public RecommendationsController(ITrackRepository trackRepository, IOpenaiService openaiService, ISpotifyService spotifyService)
    {
        _trackRepository = trackRepository;
        _openaiService = openaiService;
        _spotifyService = spotifyService;
    }

    [HttpGet("/recommendations")]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet("/recommendations/playlist/{id:guid}")]
    public async Task<IActionResult> Playlist([FromRoute] Guid id)
    {
        return View();
    }

    [HttpGet("/api/recommendations/song")]
    public async Task<IActionResult> GetSongRecommendation()
    {
        var tracks = await _trackRepository.GetAllAsync();

        string jsonSchema = System.IO.File.ReadAllText("Schemas/aiTrackDto.json");

        var completion = await _openaiService.GetAiTrackAsync(jsonSchema, tracks.ToList());

        var track = await _spotifyService.GetTrackByTitleAndArtistAsync(User.FindFirstValue("access_token"), completion.Title, completion.Artist, completion.Genre);

        // Save track recommendation to database

        return Ok(track);
    }

    [HttpGet("/api/recommendations/playlist")]
    public async Task<IActionResult> GetPlaylistRecommendation()
    {
        var userTracks = await _trackRepository.GetAllAsync();

        string jsonSchema = System.IO.File.ReadAllText("Schemas/aiPlaylistDto.json");

        var completion = await _openaiService.GetAiPlaylistAsync(jsonSchema, userTracks.ToList());

        List<Track> sugggestedTracks = new();

        foreach (var track in completion.Tracks)
        {
            var spotifyTrack = await _spotifyService.GetTrackByTitleAndArtistAsync(User.FindFirstValue("access_token"), track.Title, track.Artist, track.Genre);

            if (spotifyTrack != null)
            {
                sugggestedTracks.Add(spotifyTrack);
            }
        }

        // save playlist recommendation to database

        return Ok(new
        {
            id = Guid.NewGuid(),
            title = completion.PlaylistName,
            description = completion.PlaylistDescription,
            tracks = sugggestedTracks
        });
    }
}