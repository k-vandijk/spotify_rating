using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using spotify_rating.Data.Repositories;
using spotify_rating.Services;
using System.Security.Claims;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecommendationsController : Controller
{
    private readonly IOpenaiService _openaiService;
    private readonly ISpotifyService _spotifyService;
    private readonly IUserTrackRepository _userTrackRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IPlaylistTrackRepository _playlistTrackRepository;
    private readonly IUserPlaylistRepository _userPlaylistRepository;

    public RecommendationsController(IOpenaiService openaiService, ISpotifyService spotifyService, IUserTrackRepository userTrackRepository, ITrackRepository trackRepository, IPlaylistRepository playlistRepository, IPlaylistTrackRepository playlistTrackRepository, IUserPlaylistRepository userPlaylistRepository)
    {
        _openaiService = openaiService;
        _spotifyService = spotifyService;
        _userTrackRepository = userTrackRepository;
        _trackRepository = trackRepository;
        _playlistRepository = playlistRepository;
        _playlistTrackRepository = playlistTrackRepository;
        _userPlaylistRepository = userPlaylistRepository;
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
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized("Access token is missing.");

        var userTracks = await _userTrackRepository.GetAllAsync();

        string jsonSchema = System.IO.File.ReadAllText("Schemas/aiTrackDto.json");

        var completion = await _openaiService.GetAiTrackAsync(jsonSchema, userTracks.ToList());

        Track track = await _spotifyService.GetTrackByTitleAndArtistAsync(accessToken, completion.Title, completion.Artist, completion.Genre);

        if (track == null)
            return NotFound("No track found matching the AI recommendation.");

        var existingTrack = await _trackRepository.GetQueryable().FirstOrDefaultAsync(t => t.SpotifyTrackId == track.SpotifyTrackId);
        if (existingTrack != null)
        {
            track = existingTrack;
            track.AiGenre = completion.Genre;
            await _trackRepository.UpdateAsync(track);
        }
        else
        {
            await _trackRepository.AddAsync(track);
        }

        return Ok(track);
    }

    [HttpGet("/api/recommendations/playlist")]
    public async Task<IActionResult> GetPlaylistRecommendation()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized("Access token is missing.");

        string? spotifyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(spotifyUserId))
            return Unauthorized("Spotify user ID is missing.");

        var userTracks = await _userTrackRepository.GetAllAsync();

        string jsonSchema = System.IO.File.ReadAllText("Schemas/aiPlaylistDto.json");

        var completion = await _openaiService.GetAiPlaylistAsync(jsonSchema, userTracks.ToList());

        var playlist = new Playlist
        {
            Name = completion.Name,
            Description = completion.Description,
            Genre = completion.Genre
        };

        await _playlistRepository.AddAsync(playlist);

        foreach (var track in completion.Tracks)
        {
            var spotifyTrack = await _spotifyService.GetTrackByTitleAndArtistAsync(accessToken, track.Title, track.Artist, track.Genre);

            if (spotifyTrack != null)
            {
                var existingTrack = await _trackRepository.GetQueryable().FirstOrDefaultAsync(t => t.SpotifyTrackId == spotifyTrack.SpotifyTrackId);

                if (existingTrack != null)
                {
                    spotifyTrack = existingTrack;
                }
                else
                {
                    await _trackRepository.AddAsync(spotifyTrack);
                }

                var playlistTrack = new PlaylistTrack
                {
                    PlaylistId = playlist.Id,
                    TrackId = spotifyTrack.Id
                };

                await _playlistTrackRepository.AddAsync(playlistTrack);
            }
        }

        await _userPlaylistRepository.AddAsync(new UserPlaylist
        {
            SpotifyUserId = spotifyUserId,
            PlaylistId = playlist.Id
        });

        return Ok(playlist);
    }
}