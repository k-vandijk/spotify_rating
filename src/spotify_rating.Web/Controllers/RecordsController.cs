using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Enums;
using spotify_rating.Data.Repositories;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class TracksController : Controller
{
    private readonly ITrackRepository _trackRepository;

    public TracksController(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }

    [HttpPost("/api/tracks/rate-track")]
    public async Task<IActionResult> RateTrack(string spotifyTrackId, int rating)
    {
        if (string.IsNullOrEmpty(spotifyTrackId))
            return BadRequest("Invalid track ID.");

        if (!TrackRatingHelper.TryConvertToRating(rating, out var ratingEnum))
            return BadRequest("Rating must be 0 (LIKE), 1 (SUPER_LIKE), or 2 (DISLIKE).");

        string? spotifyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(spotifyUserId))
            return Unauthorized("Spotify user ID is missing.");

        var track = await _trackRepository.GetQueryable().FirstOrDefaultAsync(t => t.SpotifyUserId == spotifyUserId && t.SpotifyTrackId == spotifyTrackId);
        if (track is null)
            return NotFound("Track not found.");

        track.Rating = ratingEnum;
        track.RatedAtUtc = DateTime.UtcNow;
        await _trackRepository.UpdateAsync(track);

        return Ok(new
        {
            Message = "Track rated successfully.",
            Trackid = spotifyTrackId,
            Rating = rating
        });
    }
}