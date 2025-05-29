using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecommendationsController : Controller
{
    [HttpGet("recommendations")]
    public IActionResult Index()
    {
        return View();
    }
}