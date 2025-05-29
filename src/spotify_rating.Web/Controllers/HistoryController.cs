using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Repositories;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class HistoryController : Controller
{
    private readonly IRecordRepository _recordRepository;

    public HistoryController(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    [HttpGet("/history")]
    public async Task<IActionResult> Index()
    {
        var records = await _recordRepository.GetAllAsync();

        var ratedRecords = records.Where(r => r.Rating != null).ToList();

        return View(ratedRecords);
    }
}