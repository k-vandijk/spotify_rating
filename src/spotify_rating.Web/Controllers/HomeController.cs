using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Models;
using spotify_rating.Web.Utils;

namespace spotify_rating.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "spotify_tracks_with_covers.csv");
        var records = CsvLoader.LoadRecords(csvPath);

        return View(records);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}