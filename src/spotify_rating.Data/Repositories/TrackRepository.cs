using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace spotify_rating.Data.Repositories;

public interface ITrackRepository : IRepository<Track>
{
}

public class TrackRepository : Repository<Track>, ITrackRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TrackRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public new async Task<IEnumerable<Track>> GetAllAsync()
    {
        string? spotifyUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(spotifyUserId))
            return [];

        return await _dbSet.Where(r => r.SpotifyUserId == spotifyUserId).ToListAsync();
    }
}
