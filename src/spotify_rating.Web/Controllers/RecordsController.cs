using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Enums;
using spotify_rating.Web.Repositories;
using spotify_rating.Web.Services;
using System.Security.Claims;

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