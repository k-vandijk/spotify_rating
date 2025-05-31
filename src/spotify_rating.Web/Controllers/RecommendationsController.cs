using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Repositories;
using spotify_rating.Web.Services;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecommendationsController : Controller
{
    private readonly IOpenaiService _openaiService;
    private readonly IRecordRepository _recordRepository;

    public RecommendationsController(IOpenaiService openaiService, IRecordRepository recordRepository)
    {
        _openaiService = openaiService;
        _recordRepository = recordRepository;
    }

    [HttpGet("/recommendations")]
    public async Task<IActionResult> Index()
    {
        var tracks = await _recordRepository.GetAllAsync();

        var completion = await _openaiService.GetAiPlaylistByGenreAsync(tracks.ToList(), "metal");

        return View(completion);
    }
}