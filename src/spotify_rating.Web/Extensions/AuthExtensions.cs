using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace spotify_rating.Web.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddSpotifyAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Spotify";
            })
            .AddCookie()
            .AddOAuth("Spotify", options =>
            {
                options.ClientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID") ?? throw new InvalidOperationException("SPOTIFY_CLIENT_ID is not set in environment variables.");
                options.ClientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET") ?? throw new InvalidOperationException("SPOTIFY_CLIENT_SECRET is not set in environment variables.");
                options.CallbackPath = new PathString("/callback");

                options.AuthorizationEndpoint = "https://accounts.spotify.com/authorize";
                options.TokenEndpoint = "https://accounts.spotify.com/api/token";
                options.UserInformationEndpoint = "https://api.spotify.com/v1/me";

                options.Scope.Add("user-read-email");
                options.Scope.Add("user-library-read");

                options.SaveTokens = true;

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "display_name");
                options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        var response = await context.Backchannel.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                        context.RunClaimActions(json.RootElement);
                    }
                };
            });

        return services;
    }
}