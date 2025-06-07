using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace spotify_rating.Data.Repositories;

public interface IUserPlaylistRepository : IBaseRepository<UserPlaylist>
{
}

public class UserPlaylistRepository : BaseRepository<UserPlaylist>, IUserPlaylistRepository
{
    public UserPlaylistRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache) : base(context, httpContextAccessor, memoryCache)
    {
    }

    public new async Task<IEnumerable<UserPlaylist>> GetAllAsync()
    {
        string userId = GetCurrentUserId();
        string cacheKey = $"{nameof(UserPlaylist)}_all_user_{userId}";

        if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<UserPlaylist> cachedData))
        {
            return cachedData;
        }

        var data = await _dbSet
            .Include(up => up.Playlist)
            .Where(up => up.Active && up.SpotifyUserId == userId)
            .ToListAsync();

        _memoryCache.Set(cacheKey, data, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return data;
    }
}