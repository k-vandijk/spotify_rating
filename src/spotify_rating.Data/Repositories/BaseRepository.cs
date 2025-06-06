using Microsoft.EntityFrameworkCore;
using spotify_rating.Data.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.Where(x => x.Active).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Active && x.Id == id);
    }

    public async Task AddAsync(TEntity entity)
    {
        string currentUserId = GetCurrentUserId();
        entity.CreatedBy = currentUserId;
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        string currentUserId = GetCurrentUserId();
        foreach (var entity in entities)
        {
            entity.CreatedBy = currentUserId;
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
    }

    public async Task RemoveAsync(TEntity entity)
    {
        string currentUserId = GetCurrentUserId();
        entity.Active = false;
        entity.DeactivatedAtUtc = DateTime.UtcNow;
        entity.DeactivatedBy = currentUserId;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _dbSet.Where(x => x.Active).AsQueryable();
    }

    private string GetCurrentUserId()
    {
        string? spotifyUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        return spotifyUserId ?? string.Empty;
    }
}