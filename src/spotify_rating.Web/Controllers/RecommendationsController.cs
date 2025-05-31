using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Data.Repositories;
using spotify_rating.Web.Services;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecommendationsController : Controller
{
    private readonly IOpenaiService _openaiService;
    private readonly ITrackRepository _trackRepository;

    public RecommendationsController(IOpenaiService openaiService, ITrackRepository trackRepository)
    {
        _openaiService = openaiService;
        _trackRepository = trackRepository;
    }

    [HttpGet("/recommendations")]
    public async Task<IActionResult> Index()
    {
        var tracks = await _trackRepository.GetAllAsync();

        var completion = await _openaiService.GetAiPlaylistByGenreAsync(tracks.ToList(), "metal");

        return View(completion);
    }
}