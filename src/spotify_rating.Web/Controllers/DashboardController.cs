using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    [HttpGet("/dashboard")]
    public IActionResult Index()
    {
        return View();
    }
}