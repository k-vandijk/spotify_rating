using Azure.AI.OpenAI;
using OpenAI.Chat;
using spotify_rating.Data.Dtos;
using spotify_rating.Data.Entities;
using System.ClientModel;
using System.Text;
using System.Text.Json;

namespace spotify_rating.Services;

public interface IOpenaiService
{
    Task<string> GetChatCompletionAsync(string prompt);
    //Task<AiPlaylistDto> GetAiPlaylistAsync(List<Track> inputTracks, string genre = null);
    Task<AiTrackDto> GetAiTrackAsync(string jsonSchema, List<Track> inputTracks, string genre = null);
}

public class OpenaiService : IOpenaiService
{
    private readonly string _apiKey;
    private readonly string _apiUrl;
    private readonly AzureOpenAIClient _azureClient;
    private readonly ChatClient _chatClient;

    public OpenaiService()
    {
        _apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY") ?? throw new InvalidOperationException("AZURE_OPENAI_KEY is not set in environment variables.");
        _apiUrl = Environment.GetEnvironmentVariable("AZURE_OPENAI_URL") ?? throw new InvalidOperationException("AZURE_OPENAI_URL is not set in environment variables.");
        
        _azureClient = new(
            new Uri(_apiUrl),
            new ApiKeyCredential(_apiKey)
        );
        
        _chatClient = _azureClient.GetChatClient("gpt-4o-mini");
    }

    public async Task<string> GetChatCompletionAsync(string prompt)
    {
        ChatCompletion completion = await _chatClient.CompleteChatAsync(new UserChatMessage(prompt));

        if (completion.Content.Count > 0)
        {
            return completion.Content[0].Text;
        }

        throw new InvalidOperationException("No content returned from OpenAI chat completion.");
    }

    //public async Task<AiPlaylistDto> GetAiPlaylistAsync(List<Track> inputTracks, string? genre)
    //{
    //    JSchemaGenerator generator = new JSchemaGenerator();
        
    //    var jsonSchema = generator.Generate(typeof(AiPlaylistDto)).ToString();

    //    var userPrompt = BuildPlaylistPrompt(inputTracks, genre);

    //    var chat = new List<ChatMessage>
    //    {
    //        new SystemChatMessage("You are a music recommendation AI. Based on a user's liked tracks, return a playlist of exactly 40 songs matching the requested genre (if supplied)."),
    //        new UserChatMessage(userPrompt)
    //    };

    //    ChatCompletion completion = await _chatClient.CompleteChatAsync(
    //        chat,
    //        new ChatCompletionOptions
    //        {
    //            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat("aiPlaylistDto", BinaryData.FromString(jsonSchema))
    //        });

    //    var result = JsonSerializer.Deserialize<AiPlaylistDto>(completion.Content[0].Text, new JsonSerializerOptions
    //    {
    //        PropertyNameCaseInsensitive = true
    //    });

    //    if (result == null)
    //        throw new Exception("Failed to deserialize playlist result.");

    //    return result;
    //}

    public async Task<AiTrackDto> GetAiTrackAsync(string jsonSchema, List<Track> inputTracks, string? genre)
    {
        var userPrompt = BuildSongPrompt(inputTracks, genre);

        var chat = new List<ChatMessage>
        {
            new SystemChatMessage("You are a music recommendation AI. Based on a user's liked tracks, return a unique track (that's not in the list) matching the requested genre (if supplied)."),
            new UserChatMessage(userPrompt)
        };

        ChatCompletion completion = await _chatClient.CompleteChatAsync(
            chat,
            new ChatCompletionOptions
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat("aiTrackDto", BinaryData.FromString(jsonSchema))
            });

        var result = JsonSerializer.Deserialize<AiTrackDto>(completion.Content[0].Text, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null)
            throw new Exception("Failed to deserialize track result.");

        return result;
    }

    private string BuildPlaylistPrompt(List<Track> ratedTracks, string? genre)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"I like the following tracks, and I'd like a playlist of 40 tracks in the genre '{genre ?? "'any applicable genre'"}':");

        foreach (var track in ratedTracks.OrderBy(rt => Guid.NewGuid()).Take(100))
        {
            sb.AppendLine($"- \"{track.Title}\" by {track.Artist}");
        }

        sb.AppendLine("Please return the playlist as a JSON object conforming to the schema I will provide.");
        return sb.ToString();
    }

    private string BuildSongPrompt(List<Track> ratedTracks, string? genre)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"I like the following tracks, and I'd like a song in the genre '{genre ?? "'any applicable genre'"}':");

        foreach (var track in ratedTracks.OrderBy(rt => Guid.NewGuid()).Take(100))
        {
            sb.AppendLine($"- \"{track.Title}\" by {track.Artist}");
        }

        sb.AppendLine("Please return the song as a JSON object conforming to the schema I will provide.");
        return sb.ToString();
    }
}