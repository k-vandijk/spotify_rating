using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Entities;
using spotify_rating.Web.Repositories;
using System.Security.Claims;
using spotify_rating.Web.Services;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISpotifyService _spotifyService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRecordRepository _recordRepository;

    public HomeController(ISpotifyService spotifyService, ILogger<HomeController> logger, IServiceProvider serviceProvider, IRecordRepository recotRepository)
    {
        _spotifyService = spotifyService;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _recordRepository = recotRepository;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet("/home/data")]
    public async Task<IActionResult> GetData()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        string? spotifyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized("Access token is missing.");

        if (string.IsNullOrEmpty(spotifyUserId))
            return Unauthorized("Spotify user ID is missing.");

        // Records van spotify en vanuit de database tegelijk ophalen
        var liveTracksTask = _spotifyService.GetLikedTracksAsync(accessToken, spotifyUserId);
        var storedTracksTask = _recordRepository.GetAllAsync();

        await Task.WhenAll(liveTracksTask, storedTracksTask);

        var liveTracks = liveTracksTask.Result;
        var storedTracks = storedTracksTask.Result;

        var shuffledRecords = GetShuffledRecords(liveTracks);

        // Op het achtergrond synchroniseren.
        _ = Task.Run(async () => await SynchronizeTracksAsync(liveTracks, storedTracks.ToList()));

        return Json(new
        {
            records = shuffledRecords,
            total = liveTracks.Count,
            rated = storedTracks.Count(r => r.Rating != null)
        });
    }

    private async Task SynchronizeTracksAsync(List<Record> liveTracks, List<Record> storedTracks)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRecordRepository>();
        var spotifyService = scope.ServiceProvider.GetRequiredService<ISpotifyService>();

        try
        {
            await AddNewTracksAsync(repository, spotifyService, liveTracks, storedTracks.ToList());

            await DeleteRemovedTracksAsync(repository, spotifyService, liveTracks, storedTracks.ToList());
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during background sync.");
        }
    }

    private static async Task AddNewTracksAsync(IRecordRepository recordRepository, ISpotifyService spotifyService, List<Record> liveTracks, List<Record> storedTracks)
    {
        var newTracks = spotifyService.GetNewTracksAsync(liveTracks, storedTracks).ToList();
        
        if (newTracks.Any())
        {
            await recordRepository.AddRangeAsync(newTracks);
        }
    }

    private static async Task DeleteRemovedTracksAsync(IRecordRepository recordRepository, ISpotifyService spotifyService, List<Record> liveTracks, List<Record> storedTracks)
    {
        var removedTracks = spotifyService.GetRemovedTracksAsync(liveTracks, storedTracks).ToList();
        
        if (removedTracks.Any())
        {
            foreach (var track in removedTracks)
            {
                await recordRepository.RemoveAsync(track);
            }
        }
    }
        
    private List<Record> GetShuffledRecords(IEnumerable<Record> records)
    {
        return records
            .Where(r => r.Rating == null)
            .OrderBy(r => Guid.NewGuid())
            .ToList();
    }
}