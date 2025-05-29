using Microsoft.EntityFrameworkCore;
using spotify_rating.Web.Entities;
using System.Security.Claims;

namespace spotify_rating.Web.Repositories;

public interface IRecordRepository : IRepository<Record>
{
}

public class RecordRepository : Repository<Record>, IRecordRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RecordRepository(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public new async Task<IEnumerable<Record>> GetAllAsync()
    {
        string? spotifyUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(spotifyUserId))
            return [];

        return await _dbSet.Where(r => r.UserId == spotifyUserId).ToListAsync();
    }
}
