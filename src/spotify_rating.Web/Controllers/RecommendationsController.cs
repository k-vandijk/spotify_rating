using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spotify_rating.Web.Entities;
using spotify_rating.Web.Repositories;
using spotify_rating.Web.Services;
using spotify_rating.Web.ViewModels;

namespace spotify_rating.Web.Controllers;

[Authorize]
public class RecommendationsController : Controller
{
    private readonly IRecordRepository _recordRepository;
    private readonly IOpenaiService _openaiService;

    public RecommendationsController(IOpenaiService openaiService, IRecordRepository recordRepository)
    {
        _openaiService = openaiService;
        _recordRepository = recordRepository;
    }

    [HttpGet("/recommendations")]
    public async Task<IActionResult> Index()
    {
        var records = await _recordRepository.GetAllAsync();

        var prompt = BuildPromptFromRecords(records);

        var recommendations = await _openaiService.GetRecommendationsAsync(prompt);

        return View(new RecommendationsViewModel
        {
            Recommendations = recommendations
        });
    }

    private string BuildPromptFromRecords(IEnumerable<Record> records)
    {
        var basePrompt = """
     
             You are a music recommendation assistant.

             Based on the following user-rated records, suggest 3 new songs in JSON format with the following structure:

             [
               {
                 "title": "Track Title",
                 "artist": "Artist Name",
                 "genre": "Genre",
                 "reason": "Why this track fits the user's taste"
               }
             ]

             ONLY return a raw JSON array. DO NOT wrap it in code blocks or markdown. DO NOT include any explanation.
             
        """;

        var formattedRecords = string.Join("\n", records.Select(r =>
            $"Title: {r.Title}, Artist: {r.Artist}, Rating: {(int?)r.Rating}"));

        return basePrompt + "\n\n" + formattedRecords;
    }
}