using spotify_rating.Data;
using spotify_rating.Web.Extensions;
using spotify_rating.Web.Middleware;
using spotify_rating.Web.Utils;

DotenvLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "../../.env"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpotifyAuthentication();

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddSqlServer<DataContext>(connectionString);

builder.Services.AddServices();

builder.Services.AddRepositories();

builder.Services.AddHandlers();

builder.Services.AddControllersWithViews(options =>
    options.Filters.Add<TrafficLoggingMiddleware>());

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseExceptionHandler("/500");
app.UseStatusCodePagesWithReExecute("/{0}");    

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();