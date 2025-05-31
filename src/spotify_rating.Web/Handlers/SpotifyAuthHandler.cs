using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace spotify_rating.Web.Handlers;

public class SpotifyAuthHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SpotifyAuthHandler> _logger;

    public SpotifyAuthHandler(
        IHttpContextAccessor httpContextAccessor,
        ILogger<SpotifyAuthHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext!;
        var accessToken = await context.GetTokenAsync("access_token");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _logger.LogWarning("Access token expired. Attempting refresh...");

            var newAccessToken = await RefreshTokenAsync(context);

            if (!string.IsNullOrEmpty(newAccessToken))
            {
                // Retry request with new token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
                response = await base.SendAsync(request, cancellationToken);
            }
        }

        return response;
    }

    private async Task<string?> RefreshTokenAsync(HttpContext context)
    {
        var refreshToken = await context.GetTokenAsync("refresh_token");
        if (string.IsNullOrEmpty(refreshToken)) return null;

        var clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID") ?? throw new InvalidOperationException("SPOTIFY_CLIENT_ID is not set in environment variables.");
        var clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET") ?? throw new InvalidOperationException("SPOTIFY_CLIENT_SECRET is not set in environment variables.");

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken
        });

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var newToken = json.RootElement.GetProperty("access_token").GetString();

        var authResult = await context.AuthenticateAsync();
        authResult.Properties.UpdateTokenValue("access_token", newToken);
        await context.SignInAsync(authResult.Principal, authResult.Properties);

        return newToken;
    }
}
