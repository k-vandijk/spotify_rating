using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Models;
using spotify_rating.Web.Services;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ISpotifyService _spotifyService;

    public HomeController(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}