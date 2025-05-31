using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Enums;
using spotify_rating.Web.Repositories;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecordsController : Controller
{
    private readonly IRecordRepository _recordRepository;

    public RecordsController(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    [HttpPost("/api/records/rate-record")]
    public async Task<IActionResult> RateRecord(string spotifyTrackId, int rating)
    {
        if (string.IsNullOrEmpty(spotifyTrackId))
            return BadRequest("Invalid track ID.");

        if (!RecordRatingHelper.TryConvertToRating(rating, out var ratingEnum))
            return BadRequest("Rating must be 0 (LIKE), 1 (SUPER_LIKE), or 2 (DISLIKE).");

        string? spotifyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(spotifyUserId))
            return Unauthorized("Spotify user ID is missing.");

        var record = await _recordRepository.GetQueryable().FirstOrDefaultAsync(t => t.SpotifyUserId == spotifyUserId && t.SpotifyTrackId == spotifyTrackId);
        if (record is null)
            return NotFound("Record not found.");

        record.Rating = ratingEnum;
        record.RatedAtUtc = DateTime.UtcNow;
        await _recordRepository.UpdateAsync(record);

        return Ok(new
        {
            Message = "Track rated successfully.",
            RecordId = spotifyTrackId,
            Rating = rating
        });
    }
}