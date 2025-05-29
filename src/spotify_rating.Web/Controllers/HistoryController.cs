using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class HistoryController : Controller
{
    [HttpGet("history")]
    public IActionResult Index()
    {
        return View();
    }
}