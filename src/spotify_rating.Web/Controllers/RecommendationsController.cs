using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Repositories;
using spotify_rating.Web.Services;
using spotify_rating.Web.ViewModels;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecommendationsController : Controller
{
    private readonly IRecordRepository _recordRepository;
    private readonly IOpenaiService _openaiService;

    public RecommendationsController(IRecordRepository recordRepository, IOpenaiService openaiService)
    {
        _recordRepository = recordRepository;
        _openaiService = openaiService;
    }

    [HttpGet("/recommendations")]
    public async Task<IActionResult> Index()
    {
        return View(new RecommendationsViewModel
        {
            Recommendations = null
        });
    }
}