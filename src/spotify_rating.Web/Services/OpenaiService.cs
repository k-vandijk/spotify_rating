using Azure;
using spotify_rating.Web.Entities;
using spotify_rating.Web.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace spotify_rating.Web.Services;

public interface IOpenaiService
{
    Task<string> GetChatCompletionAsync(string prompt);
}

public class OpenaiService : IOpenaiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiUrl;

    public OpenaiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY") ?? throw new InvalidOperationException("AZURE_OPENAI_KEY is not set in environment variables.");
        _apiUrl = Environment.GetEnvironmentVariable("AZURE_OPENAI_URL") ?? throw new InvalidOperationException("AZURE_OPENAI_URL is not set in environment variables.");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> GetChatCompletionAsync(string prompt)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl);
        request.Content = new StringContent(
            JsonSerializer.Serialize(new
            {
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                max_tokens = 1000,
                temperature = 0.7
            }),
            Encoding.UTF8,
            "application/json"
        );

        var completion = await _httpClient.SendAsync(request);

        if (!completion.IsSuccessStatusCode)
        {
            var errorContent = await completion.Content.ReadAsStringAsync();
            throw new HttpRequestException($"OpenAI API request failed with status code {completion.StatusCode}: {errorContent}");
        }

        var responseContent = await completion.Content.ReadAsStringAsync();
        
        var responseJson = JsonSerializer.Deserialize<JsonElement>(responseContent);

        if (responseJson.TryGetProperty("choices", out var choices) && choices[0].TryGetProperty("message", out var message))
        {
            return message.GetProperty("content").GetString() ?? string.Empty;
        }

        throw new InvalidOperationException("Unexpected response format from OpenAI API.");
    }
}