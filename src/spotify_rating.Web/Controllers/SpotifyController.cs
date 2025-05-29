using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Services;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class SpotifyController : Controller
{
    private readonly ISpotifyService _spotifyService;
    
    public SpotifyController(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    [HttpGet("/api/spotify/liked-tracks")]
    public async Task<IActionResult> GetLikedTracks()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var tracks = await _spotifyService.GetLikedTracksAsync(accessToken);

        return Ok(tracks);
    }
}