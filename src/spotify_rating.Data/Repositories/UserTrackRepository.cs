using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace spotify_rating.Data.Repositories;

public interface IUserTrackRepository : IBaseRepository<UserTrack>
{
}

public class UserTrackRepository : BaseRepository<UserTrack>, IUserTrackRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserTrackRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache) : base(context, httpContextAccessor, memoryCache)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public new async Task<IEnumerable<UserTrack>> GetAllAsync()
    {
        string userId = GetCurrentUserId();
        string cacheKey = $"{nameof(UserTrack)}_all_user_{userId}";

        if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<UserTrack> cachedData))
        {
            return cachedData;
        }

        var data = await _dbSet
            .Include(ut => ut.Track)
            .Where(ut => ut.Active && ut.SpotifyUserId == userId)
            .ToListAsync();

        _memoryCache.Set(cacheKey, data, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return data;
    }
}
