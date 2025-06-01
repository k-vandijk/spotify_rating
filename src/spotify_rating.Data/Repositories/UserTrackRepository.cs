using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace spotify_rating.Data.Repositories;

public interface IUserTrackRepository : IRepository<UserTrack>
{
}

public class UserTrackRepository : Repository<UserTrack>, IUserTrackRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserTrackRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public new async Task<IEnumerable<UserTrack>> GetAllAsync()
    {
        string? spotifyUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(spotifyUserId))
            return [];

        return await _dbSet.Include(ut => ut.Track).Where(r => r.SpotifyUserId == spotifyUserId).ToListAsync();
    }
}
