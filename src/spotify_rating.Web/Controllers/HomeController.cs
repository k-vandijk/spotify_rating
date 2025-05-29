using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Entities;
using spotify_rating.Web.Repositories;
using spotify_rating.Web.ViewModels;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IRecordRepository _recordRepository;

    public HomeController(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
        var records = await _recordRepository.GetAllAsync();
        
        var shuffledRecords = records
            .Where(r => r.Rating == null)
            .OrderBy(r => Guid.NewGuid())
            .ToList();

        return View(new HomeViewModel
        {
            Records = shuffledRecords,
            TotalRecords = records.Count(),
            RatedRecords = records.Count() - shuffledRecords.Count()
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}