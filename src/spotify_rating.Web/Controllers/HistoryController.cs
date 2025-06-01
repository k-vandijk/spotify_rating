using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Data.Repositories;
using spotify_rating.Web.ViewModels;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class HistoryController : Controller
{
    private readonly IUserTrackRepository _userTrackRepository;

    public HistoryController(IUserTrackRepository userTrackRepository)
    {
        _userTrackRepository = userTrackRepository;
    }

    [HttpGet("/history")]
    public async Task<IActionResult> Index()
    {
        var userTracks = await _userTrackRepository.GetAllAsync();

        var ratedTracks = userTracks.Where(r => r.Rating != null).ToList();

        return View(new HistoryViewModel
        {
            Tracks = ratedTracks.ToList(),
            Total = userTracks.ToList().Count,
            Rated = ratedTracks.Count
        });
    }
}