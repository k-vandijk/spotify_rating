using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using spotify_rating.Data.Enums;
using spotify_rating.Data.Repositories;
using System.Security.Claims;
using spotify_rating.Data.Dtos;
using spotify_rating.Services;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISpotifyService _spotifyService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserTrackRepository _userTrackRepository;

    public HomeController(ISpotifyService spotifyService, ILogger<HomeController> logger, IServiceProvider serviceProvider, IUserTrackRepository userTrackRepository)
    {
        _spotifyService = spotifyService;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _userTrackRepository = userTrackRepository;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet("/api/home/data")]
    public async Task<IActionResult> GetData()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized("Access token is missing.");

        string? spotifyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(spotifyUserId))
            return Unauthorized("Spotify user ID is missing.");

        // Tracks van spotify en vanuit de database tegelijk ophalen
        var liveLikedTracksTask = _spotifyService.GetLikedTracksAsync(accessToken, spotifyUserId);
        var storedUserTracksTask = _userTrackRepository.GetAllAsync();

        await Task.WhenAll(liveLikedTracksTask, storedUserTracksTask);

        var liveLikedTracks = liveLikedTracksTask.Result;
        var storedUserTracks = storedUserTracksTask.Result;

        var shuffledTracks = GetShuffledUnratedTracks(liveLikedTracks, storedUserTracks.ToList());

        // Synchronize tracks in the background
        _ = Task.Run(() => SynchronizeTracksAsync(_serviceProvider, liveLikedTracks, storedUserTracks.ToList(), spotifyUserId));

        return Json(new
        {
            tracks = shuffledTracks,
            total = liveLikedTracks.Count,
            rated = storedUserTracks.Count(r => r.Rating != null)
        });
    }

    private static List<Track> GetShuffledUnratedTracks(List<Track> liveTracks, List<UserTrack> userTracks)
    {
        return liveTracks
            .Where(lt => !userTracks.Any(ut => ut.Track.SpotifyTrackId == lt.SpotifyTrackId && ut.Rating != null))
            .OrderBy(r => Guid.NewGuid())
            .ToList();
    }

    private static async Task SynchronizeTracksAsync(IServiceProvider serviceProvider, List<Track> liveLikedTracks, List<UserTrack> storedUserTracks, string spotifyUserId)
    {
        using var scope = serviceProvider.CreateScope();
        var trackRepository = scope.ServiceProvider.GetRequiredService<ITrackRepository>();
        var userTrackRepository = scope.ServiceProvider.GetRequiredService<IUserTrackRepository>();

        // 1) Store new spotify tracks to Tracks
        await PersistNewTracksAsync(trackRepository, liveLikedTracks);

        // 2) Add new user tracks to UserTracks
        await PersistNewUserTracksAsync(liveLikedTracks, storedUserTracks, spotifyUserId, userTrackRepository);
    }

    private static async Task PersistNewTracksAsync(ITrackRepository trackRepository, List<Track> liveLikedTracks)
    {
        var storedTracks = await trackRepository.GetAllAsync();
        var liveLikedTracksThatAreNotStored = liveLikedTracks
            .Where(lt => storedTracks.All(st => st.SpotifyTrackId != lt.SpotifyTrackId))
            .ToList();

        if (liveLikedTracksThatAreNotStored.Any())
        {
            await trackRepository.AddRangeAsync(liveLikedTracksThatAreNotStored);
        }
    }

    private static async Task PersistNewUserTracksAsync(List<Track> liveLikedTracks, List<UserTrack> storedUserTracks, string spotifyUserId, IUserTrackRepository userTrackRepository)
    {
        var newUserTracks = liveLikedTracks
            .Where(lt => storedUserTracks.All(ut => ut.Track.SpotifyTrackId != lt.SpotifyTrackId))
            .Select(lt => new UserTrack
            {
                SpotifyUserId = spotifyUserId,
                Track = lt,
            })
            .ToList();

        if (newUserTracks.Any())
        {
            await userTrackRepository.AddRangeAsync(newUserTracks);
        }
    }

    [HttpPost("/api/home/rate-track")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RateTrack([FromBody] RateTrackDto dto)
    {
        if (string.IsNullOrEmpty(dto.SpotifyTrackId))
            return BadRequest("Invalid track ID.");

        if (!TrackRatingHelper.TryConvertToRating(dto.Rating, out var ratingEnum))
            return BadRequest("Rating must be 0 (LIKE), 1 (SUPER_LIKE), or 2 (DISLIKE).");

        string? spotifyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(spotifyUserId))
            return Unauthorized("Spotify user ID is missing.");

        var userTrack = await _userTrackRepository.GetQueryable().FirstOrDefaultAsync(ut => ut.SpotifyUserId == spotifyUserId && ut.Track.SpotifyTrackId == dto.SpotifyTrackId);
        
        if (userTrack is null)
            return NotFound("Track not found.");

        userTrack.Rating = ratingEnum;
        userTrack.RatedAtUtc = DateTime.UtcNow;

        await _userTrackRepository.UpdateAsync(userTrack);

        return Ok(new
        {
            Message = "Track rated successfully.",
            Trackid = dto.SpotifyTrackId,
            Rating = dto.Rating
        });
    }

    [HttpGet("404")]
    [AllowAnonymous]
    public IActionResult NotFoundPage()
    {
        TempData["HideLayout"] = "true";
        return View();
    }

    [HttpGet("401")]
    [AllowAnonymous]
    public IActionResult UnauthorizedPage()
    {
        TempData["HideLayout"] = "true";
        return View();
    }

    [HttpGet("500")]
    [AllowAnonymous]
    public IActionResult ErrorPage()
    {
        TempData["HideLayout"] = "true";
        return View();
    }
}