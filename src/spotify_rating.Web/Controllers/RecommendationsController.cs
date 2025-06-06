using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using spotify_rating.Data.Repositories;
using spotify_rating.Services;
using System.Security.Claims;
using spotify_rating.Web.ViewModels;

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
        var userTracks = _userTrackRepository.GetQueryable()
            .Include(ut => ut.Track)
            .Where(ut => ut.IsAiSuggestion && !ut.IsDismissed)
            .ToList();

        var userPlaylists = _userPlaylistRepository.GetQueryable()
            .Include(up => up.Playlist)
            .Where(up => up.IsAiSuggestion && !up.IsDismissed)
            .ToList();

        return View(new RecommendationsViewModel
        {
            Tracks = userTracks.Select(ut => ut.Track).ToList(),
            Playlists = userPlaylists.Select(up => up.Playlist).ToList()
        });
    }

    [HttpGet("/recommendations/playlist/{id:guid}")]
    public async Task<IActionResult> Playlist([FromRoute] Guid id)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id);
        
        if (playlist == null)
            return NotFound("Playlist not found.");

        var playlistTracks = await _playlistTrackRepository.GetQueryable().Include(pt => pt.Track)
            .Where(pt => pt.PlaylistId == playlist.Id).ToListAsync();

        return View(new PlaylistViewModel
        {
            Playlist = playlist,
            Tracks = playlistTracks.Select(pt => pt.Track).ToList()
        });
    }

    [HttpGet("/api/recommendations/song")]
    public async Task<IActionResult> GetSongRecommendation()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized("Access token is missing.");

        string? spotifyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(spotifyUserId))
            return Unauthorized("Spotify user ID is missing.");

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

        await _userTrackRepository.AddAsync(new UserTrack
        {
            SpotifyUserId = spotifyUserId,
            TrackId = track.Id,
            IsAiSuggestion = true,
        });

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
                    existingTrack.AiGenre = spotifyTrack.AiGenre;
                    await _trackRepository.UpdateAsync(existingTrack);
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
            PlaylistId = playlist.Id,
            IsAiSuggestion = true
        });

        return Ok(playlist);
    }

    [HttpPost("/api/recommendations/render-track")]
    public IActionResult RenderListItem([FromBody] Track track)
    {
        var viewModel = new ListItemViewModel
        {
            PictureUrl = track.SpotifyAlbumCoverUrl,
            Title = track.Title,
            Subtitle = track.Artist,
            Badge = track.AiGenre,
            SpotifyButtonUrl = track.SpotifyUri,
            LikeButtonUrl = $"/api/recommendations/like?track={track.SpotifyTrackId}",
            DislikeButtonUrl = $"/api/recommendations/dislike?track={track.SpotifyTrackId}"
        };

        return PartialView("Components/_ListItem", viewModel);
    }

    [HttpPost("/api/recommendations/render-playlist")]
    public IActionResult RenderListItem([FromBody] Playlist playlist)
    {
        var viewModel = new ListItemMiniViewModel
        {
            CardUrl = $"/recommendations/playlist/{playlist.Id}",
            Title = playlist.Name,
            Subtitle = playlist.Description,
            Badge = playlist.Genre,
            BadgeClass = "bg-info"
        };

        return PartialView("Components/_ListItemMini", viewModel);
    }

    [HttpGet("/api/recommendations/like")]
    public async Task<IActionResult> LikeRecommendation(string track)
    {
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/api/recommendations/dislike")]
    public async Task<IActionResult> DislikeRecommendation(string track)
    {
        string? spotifyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(spotifyUserId))
            return Unauthorized("Spotify user ID is missing.");

        var userTrack = await _userTrackRepository.GetQueryable()
            .FirstOrDefaultAsync(ut => ut.SpotifyUserId == spotifyUserId && ut.Track.SpotifyTrackId == track);

        if (userTrack == null)
            return NotFound("Track not found.");

        userTrack.IsDismissed = true;
        userTrack.DismissedAtUtc = DateTime.UtcNow;
        await _userTrackRepository.UpdateAsync(userTrack);
        return RedirectToAction(nameof(Index));
    }
}