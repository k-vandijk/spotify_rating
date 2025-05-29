using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Entities;
using spotify_rating.Web.Enums;
using spotify_rating.Web.Repositories;
using spotify_rating.Web.Services;
using System.Security.Claims;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecordsController : Controller
{
    private readonly IRecordRepository _recordRepository;
    private readonly ISpotifyService _spotifyService;

    public RecordsController(ISpotifyService spotifyService, IRecordRepository recordRepository)
    {
        _spotifyService = spotifyService;
        _recordRepository = recordRepository;
    }

    //[HttpGet("/api/records/liked-records")]
    //public async Task<IActionResult> GetLikedRecords()
    //{
    //    var accessToken = await HttpContext.GetTokenAsync("access_token");
        
    //    string? spotifyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //    if (string.IsNullOrEmpty(accessToken))
    //        return Unauthorized("Access token is missing.");

    //    if (string.IsNullOrEmpty(spotifyUserId))
    //        return Unauthorized("Spotify user ID is missing.");

    //    var tracks = await _spotifyService.GetLikedTracksAsync(accessToken, spotifyUserId);

    //    await _recordRepository.AddRangeAsync(tracks);

    //    return Ok(tracks);
    //}

    [HttpPost("/api/records/rate-record")]
    public async Task<IActionResult> RateRecord(Guid recordId, int rating)
    {
        if (recordId == Guid.Empty)
            return BadRequest("Invalid record ID.");

        if (!RecordRatingHelper.TryConvertToRating(rating, out var ratingEnum))
            return BadRequest("Rating must be 0 (LIKE), 1 (SUPER_LIKE), or 2 (DISLIKE).");

        var record = await _recordRepository.GetByIdAsync(recordId);
        if (record is null)
            return NotFound("Record not found.");

        record.Rating = ratingEnum;
        record.RatedAtUtc = DateTime.UtcNow;
        await _recordRepository.UpdateAsync(record);

        return Ok(new
        {
            Message = "Record rated successfully.",
            RecordId = recordId,
            Rating = rating
        });
    }
}