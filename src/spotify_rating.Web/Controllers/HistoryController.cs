using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Data.Repositories;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class HistoryController : Controller
{
    private readonly ITrackRepository _trackRepository;

    public HistoryController(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }

    [HttpGet("/history")]
    public async Task<IActionResult> Index()
    {
        var tracks = await _trackRepository.GetAllAsync();

        var ratedTracks = tracks.Where(r => r.Rating != null).ToList();

        return View(ratedTracks);
    }
}