using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spotify_rating.Data;

namespace spotify_rating.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var trafficLogs = await _context.TrafficLogs.Where(tl => tl.IPAddress != "127.0.0.1").ToListAsync();

        return View(trafficLogs);
    }
}
