using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.ViewModels;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecommendationsController : Controller
{
    [HttpGet("/recommendations")]
    public async Task<IActionResult> Index()
    {
        return View(new RecommendationsViewModel
        {
            Recommendations = null
        });
    }
}