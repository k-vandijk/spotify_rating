using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace spotify_rating.Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    IQueryable<TEntity> GetQueryable();
}

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DataContext _context;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IMemoryCache _memoryCache;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _memoryCache = memoryCache;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        string userId = GetCurrentUserId();
        string cacheKey = $"{typeof(TEntity).Name}_all_user_{userId}";

        if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<TEntity> cachedData))
        {
            return cachedData;
        }

        var data = await _dbSet.Where(x => x.Active).ToListAsync();

        _memoryCache.Set(cacheKey, data, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return data;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        string userId = GetCurrentUserId();
        string cacheKey = $"{typeof(TEntity).Name}_id_{id}_user_{userId}";

        if (_memoryCache.TryGetValue(cacheKey, out TEntity cachedEntity))
        {
            return cachedEntity;
        }

        var entity = await _dbSet.FirstOrDefaultAsync(x => x.Active && x.Id == id);

        if (entity != null)
        {
            _memoryCache.Set(cacheKey, entity, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }

        return entity;
    }

    public async Task AddAsync(TEntity entity)
    {
        string currentUserId = GetCurrentUserId();
        entity.CreatedBy = currentUserId;

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        InvalidateUserCache(currentUserId, entity.Id);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        string currentUserId = GetCurrentUserId();

        foreach (var entity in entities)
        {
            entity.CreatedBy = currentUserId;
                
            InvalidateUserCache(currentUserId, entity.Id);
        }

        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        string currentUserId = GetCurrentUserId();
        
        entity.UpdatedBy = currentUserId;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();

        InvalidateUserCache(currentUserId, entity.Id);
    }

    public async Task RemoveAsync(TEntity entity)
    {
        string currentUserId = GetCurrentUserId();

        entity.Active = false;
        entity.DeactivatedAtUtc = DateTime.UtcNow;
        entity.DeactivatedBy = currentUserId;

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();

        InvalidateUserCache(currentUserId, entity.Id);
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _dbSet.Where(x => x.Active).AsQueryable();
    }

    public string GetCurrentUserId()
    {
        string? spotifyUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        return spotifyUserId ?? string.Empty;
    }

    private void InvalidateUserCache(string userId, Guid entityId)
    {
        string listCacheKey = $"{typeof(TEntity).Name}_all_user_{userId}";
        string idCacheKey = $"{typeof(TEntity).Name}_id_{entityId}_user_{userId}";

        _memoryCache.Remove(listCacheKey);
        _memoryCache.Remove(idCacheKey);
    }
}