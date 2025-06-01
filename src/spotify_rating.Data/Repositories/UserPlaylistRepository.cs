using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using System.Security.Claims;

namespace spotify_rating.Data.Repositories;

public interface IUserPlaylistRepository : IRepository<UserPlaylist>
{
}

public class UserPlaylistRepository : Repository<UserPlaylist>, IUserPlaylistRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserPlaylistRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public new async Task<IEnumerable<UserPlaylist>> GetAllAsync()
    {
        string? spotifyUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(spotifyUserId))
            return [];

        return await _dbSet.Where(r => r.SpotifyUserId == spotifyUserId).ToListAsync();
    }
}