using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Data.Repositories;
using spotify_rating.Web.ViewModels;

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

        return View(new HistoryViewModel
        {
            Tracks = ratedTracks.ToList(),
            Total = tracks.ToList().Count,
            Rated = ratedTracks.Count
        });
    }
}