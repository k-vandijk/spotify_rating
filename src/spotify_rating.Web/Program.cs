using spotify_rating.Web.Extensions;
using spotify_rating.Web.Utils;

DotenvLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "../../.env"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpotifyAuthentication();

builder.Services.AddServices();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
