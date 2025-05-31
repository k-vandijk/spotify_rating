using spotify_rating.Data;
using spotify_rating.Web.Extensions;
using spotify_rating.Web.Utils;

DotenvLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "../../.env"));

var builder = WebApplication.CreateBuilder(args);

// Add Spotify Authentication
builder.Services.AddSpotifyAuthentication();

// Add Database
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddSqlServer<DataContext>(connectionString);

// Add Services
builder.Services.AddServices();

// Add Repositories
builder.Services.AddRepositories();

// Add Handlers
builder.Services.AddHandlers();

// Add Controllers & Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

//// Global error handler (should be before routing and authentication)
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
//else
//{
//    app.UseDeveloperExceptionPage();
//}
app.UseExceptionHandler();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
