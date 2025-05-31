using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Data.Repositories;
using spotify_rating.Services;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecommendationsController : Controller
{
    private readonly ITrackRepository _trackRepository;
    private readonly IOpenaiService _openaiService;

    public RecommendationsController(ITrackRepository trackRepository, IOpenaiService openaiService)
    {
        _trackRepository = trackRepository;
        _openaiService = openaiService;
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

        var completion = await _openaiService.GetAiTrackAsync(tracks.ToList());

        // save track recommendation to database

        return Ok(completion);
    }

    [HttpGet("/api/recommendations/playlist")]
    public async Task<IActionResult> GetPlaylistRecommendation()
    {
        var tracks = await _trackRepository.GetAllAsync();

        var completion = await _openaiService.GetAiPlaylistAsync(tracks.ToList());

        // save playlist recommendation to database

        return Ok(completion);
    }
}